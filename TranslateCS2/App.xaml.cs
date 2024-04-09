using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Shell;

using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Unity;

using TranslateCS2.Brokers;
using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Ribbons;
using TranslateCS2.Core;
using TranslateCS2.Core.Brokers;
using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Properties;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Services.Databases;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Databases;
using TranslateCS2.Edits;
using TranslateCS2.ExImport;
using TranslateCS2.Sessions;
using TranslateCS2.TextSearch;
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
    private IViewConfigurations _viewConfigurations;
    private readonly ITranslationsDatabaseService _translationsDatabaseService;


    public App() : base() {
        FrameworkElement.LanguageProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(
                XmlLanguage.GetLanguage(
                    CultureInfo.CurrentCulture.IetfLanguageTag
                    )
                )
            );
        this._translationsDatabaseService = new TranslationsDB();
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
    protected override void OnStartup(StartupEventArgs e) {
        // backup first; no need to backup newly created database
        this._translationsDatabaseService.BackUpIfExists(DatabaseBackUpIndicators.APP_STARTED);
        this._translationsDatabaseService.CreateIfNotExists();
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
    protected override void RegisterTypes(IContainerRegistry containerRegistry) {
        this._viewConfigurations = new ViewConfigurations(containerRegistry);
        containerRegistry.RegisterSingleton<ITranslationsDatabaseService>(() => this._translationsDatabaseService);
        containerRegistry.RegisterSingleton<IAppCloseBrokers>(AppCloseBrokers.GetInstance);
        containerRegistry.RegisterSingleton<IViewConfigurations>(() => this._viewConfigurations);

        ViewModelLocationProvider.Register<AppRibbonControl, AppRibbonControlContext>();
    }

    /// <summary>
    ///     <br/>
    ///     <br/>
    ///     <seealso href="https://docs.prismlibrary.com/docs/modularity/index.html"/>
    ///     <br/>
    ///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Code"/>
    ///     <br/>
    ///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Directory"/>
    /// </summary>
    protected override IModuleCatalog CreateModuleCatalog() {
        if (Path.Exists(AppConfigurationManager.AppModuleDirectory)) {
            // module path has to exists and must not be null or empty, otherwise an exception is thrown
            DirectoryModuleCatalog directoryModuleCatalog = new DirectoryModuleCatalog {
                ModulePath = AppConfigurationManager.AppModuleDirectory
            };
            return directoryModuleCatalog;
        }
        return base.CreateModuleCatalog();
    }

    /// <summary>
    ///     Step: 3
    ///     <br/>
    ///     <br/>
    ///     <seealso href="https://docs.prismlibrary.com/docs/modularity/index.html"/>
    ///     <br/>
    ///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Code"/>
    ///     <br/>
    ///     <seealso href="https://github.com/PrismLibrary/Prism-Samples-Wpf/tree/master/07-Modules-Directory"/>
    /// </summary>
    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog) {
        moduleCatalog.AddModule<CoreModule>();

        // if you want to build/publish this app with your TranslatorModule
        // add a reference to your Translator-Module-Project
        // and add this module here (after CoreModule is added!)
        //moduleCatalog.AddModule<TranslatorsExampleModule>();

        moduleCatalog.AddModule<TextSearchModule>();
        moduleCatalog.AddModule<SessionsModule>();
        moduleCatalog.AddModule<EditsModule>();
        moduleCatalog.AddModule<ExImportModule>();

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
        ITranslationSessionManager translationSessionManager = this.Container.Resolve<ITranslationSessionManager>();
        IRegionManager regionManager = this.Container.Resolve<IRegionManager>();
        regionManager.RegisterViewWithRegion<AppRibbonControl>(AppConfigurationManager.AppRibbonBarRegion);

        //regionManager.RegisterViewWithRegion<>();
        // modules are initilized
        IViewConfiguration startView = this.ConfigureViews(regionManager);
        regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, startView.NavigationTarget);
        base.OnInitialized();
    }

    private IViewConfiguration ConfigureViews(IRegionManager regionManager) {
        // INFO: ViewConfigurations für Tabs
        ViewConfigurations viewConfigurations = (ViewConfigurations) this.Container.Resolve<IViewConfigurations>();
        // startview is always useable!!!
        IViewConfiguration startView = IViewConfiguration.Create<StartView, StartViewModel>(I18NRibbon.Start,
                                                                                           ImageResources.home,
                                                                                           true,
                                                                                           true);
        startView.NavToggleButton.IsChecked = true;
        viewConfigurations.AddStartViewConfiguration(startView);
        // credits view is always useable!!!
        viewConfigurations.Add(IViewConfiguration.Create<CreditsView, CreditsViewModel>(I18NRibbon.Credits, ImageResources.person_circle, true, true));
        viewConfigurations.Register(regionManager);
        return startView;
    }
}