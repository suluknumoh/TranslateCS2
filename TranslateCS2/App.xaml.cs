using System;
using System.Globalization;
using System.Net.Http;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Shell;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;

using TranslateCS2.Brokers;
using TranslateCS2.Configurations;
using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Exports;
using TranslateCS2.Controls.Imports;
using TranslateCS2.Controls.Ribbons;
using TranslateCS2.Controls.Sessions;
using TranslateCS2.Helpers;
using TranslateCS2.Models.Sessions;
using TranslateCS2.Properties;
using TranslateCS2.Services;
using TranslateCS2.ViewModels.Works;
using TranslateCS2.Views;
using TranslateCS2.Views.Works;

namespace TranslateCS2;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : PrismApplication {
    private readonly int resizeBorderThickness = 12;
    private Thickness defaultResizeBorderThickness;
    private ViewConfigurations _viewConfigurations;
    private ViewConfigurations GetViewConfigurations() {
        return this._viewConfigurations;
    }



    /// <summary>
    ///     INFO: https://learn.microsoft.com/de-de/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
    /// </summary>
    private readonly HttpClient _httpClient;

    private HttpClient GetHttpClient() {
        return this._httpClient;
    }



    public App() : base() {
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag
                    )
                )
            );
        this._httpClient = new HttpClient();
    }
    private void StateChanged(object? sender, EventArgs e) {
        MainWindow? mainWindow = sender as MainWindow;
        WindowChrome windowChrome = WindowChrome.GetWindowChrome(mainWindow);
        if (mainWindow?.WindowState == WindowState.Maximized) {
            this.defaultResizeBorderThickness = windowChrome.ResizeBorderThickness;
            windowChrome.ResizeBorderThickness = new Thickness(this.resizeBorderThickness,
                                                               this.resizeBorderThickness,
                                                               this.resizeBorderThickness,
                                                               this.resizeBorderThickness);
        } else {
            windowChrome.ResizeBorderThickness = this.defaultResizeBorderThickness;
        }
    }

    /// <summary>
    ///     Step: 1
    /// </summary>
    /// <param name="e"></param>
    protected override void OnStartup(StartupEventArgs e) {
        DatabaseHelper.CreateIfNotExists();
        if (e.Args is not null && e.Args.Length > 0) {
            string arg = e.Args[0];
            // ability to add start parameters
            //App.Current.Resources.Add(, arg);
        }
        base.OnStartup(e);
    }

    /// <summary>
    ///     Step: 2
    /// </summary>
    /// <param name="containerRegistry"></param>
    protected override void RegisterTypes(IContainerRegistry containerRegistry) {
        this._viewConfigurations = new ViewConfigurations(containerRegistry);
        containerRegistry.RegisterSingleton<HttpClient>(this.GetHttpClient);
        containerRegistry.RegisterSingleton<LatestVersionCheckService>();
        containerRegistry.RegisterSingleton<InstallPathDetector>();
        containerRegistry.RegisterSingleton<LocalizationFilesService>();
        containerRegistry.RegisterSingleton<IAppCloseBrokers>(AppCloseBrokers.GetInstance);
        containerRegistry.RegisterSingleton<ViewConfigurations>(this.GetViewConfigurations);
        containerRegistry.RegisterSingleton<TranslationSessionManager>();
        containerRegistry.Register<JSONService>();
        containerRegistry.Register<ExImportService>();
        containerRegistry.Register<FiltersService>();
        {
            // configure controls
            containerRegistry.RegisterForNavigation<NewEditSessionControl, NewEditSessionControlContext>(nameof(NewEditSessionControl));
            containerRegistry.RegisterForNavigation<ExportControl, ExportControlContext>(nameof(ExportControl));
            containerRegistry.RegisterForNavigation<ImportControl, ImportControlContext>(nameof(ImportControl));
        }
        ViewModelLocationProvider.Register<AppRibbonControl, AppRibbonControlContext>();
    }

    /// <summary>
    ///     Step: 3
    /// </summary>
    /// <param name="moduleCatalog"></param>
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
        //moduleCatalog.AddModule<>();
        // modules still not started!!!
        base.ConfigureModuleCatalog(moduleCatalog);
    }
    /// <summary>
    ///     Step: 4
    /// </summary>
    /// <returns></returns>
    protected override Window CreateShell() {
        MainWindow mainWindow = this.Container.Resolve<MainWindow>();
        mainWindow.StateChanged += this.StateChanged;
        return mainWindow;
    }
    /// <summary>
    ///     Step: 5
    /// </summary>
    protected override void OnInitialized() {
        TranslationSessionManager translationSessionManager = this.Container.Resolve<TranslationSessionManager>();
        IRegionManager regionManager = this.Container.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion<AppRibbonControl>(AppConfigurationManager.AppRibbonBarRegion);
        //regionManager.RegisterViewWithRegion<>();
        // modules are initilized
        IViewConfiguration startView = this.ConfigureViews(regionManager, translationSessionManager);
        regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, startView.NavigationTarget);
        base.OnInitialized();
    }
    private IViewConfiguration ConfigureViews(IRegionManager regionManager, TranslationSessionManager translationSessionManager) {
        // INFO: ViewConfigurations für Tabs
        ViewConfigurations viewConfigurations = this.Container.Resolve<ViewConfigurations>();
        // startview is always useable!!!
        ViewConfiguration<StartView, StartViewModel> startView = new ViewConfiguration<StartView, StartViewModel>(I18N.StringStartCap,
                                                                                                                  ImageResources.home,
                                                                                                                  true,
                                                                                                                  true);
        startView.NavToggleButton.IsChecked = true;
        viewConfigurations.Add(startView);
        viewConfigurations.Add(new ViewConfiguration<SessionManagement, SessionManagementViewModel>(I18N.StringSessionsCap, ImageResources.clock_toolbox, true, false, translationSessionManager));
        viewConfigurations.Add(new ViewConfiguration<EditDefaultView, EditDefaultViewModel>(I18N.StringEditCap, ImageResources.translate, false, false, translationSessionManager));
        viewConfigurations.Add(new ViewConfiguration<EditOccurancesView, EditOccurancesViewModel>(I18N.StringEditByOccurancesCap, ImageResources.translate, false, false, translationSessionManager));
        viewConfigurations.Add(new ViewConfiguration<ExImPortView, ExImPortViewModel>(I18N.StringExImportCap, ImageResources.database_multiple, false, false, translationSessionManager));
        if (false) {
            // disabled - under construction
            // settings view is always useable!!!
            viewConfigurations.Add(new ViewConfiguration<SettingsView, SettingsViewModel>(I18N.StringSettingsCap, ImageResources.settings, true, false, translationSessionManager));
        }
        // credits view is always useable!!!
        viewConfigurations.Add(new ViewConfiguration<CreditsView, CreditsViewModel>(I18N.StringCreditsCaps, ImageResources.person_circle, true, true));
        viewConfigurations.Register(regionManager);
        return startView;
    }
}