using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls.Ribbon;

using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Properties;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Ribbons.Sessions;
using TranslateCS2.Core.ViewModels;
using TranslateCS2.ExImport.Controls.Exports;
using TranslateCS2.ExImport.Controls.Imports;
using TranslateCS2.ExImport.Properties.I18N;
using TranslateCS2.ExImport.ViewModels.Dialogs;
using TranslateCS2.ExImport.Views.Dialogs;

namespace TranslateCS2.ExImport.ViewModels;
internal class ExImPortViewModel : ABaseViewModel {
    private readonly IContainerProvider containerProvider;
    private readonly IRegionManager regionManager;
    private readonly IViewConfigurations viewConfigurations;
    private readonly IDialogService dialogService;
    private readonly RibbonGroup subNavigation;
    private readonly RibbonGroup modInfo;
    private RibbonToggleButton subNavExport;
    private RibbonToggleButton subNavImport;
    private Type current = typeof(ExportControl);

    public ExImPortViewModel(IContainerProvider containerProvider,
                             IRegionManager regionManager,
                             IViewConfigurations viewConfigurations,
                             IDialogService dialogService) {
        this.containerProvider = containerProvider;
        this.regionManager = regionManager;
        this.viewConfigurations = viewConfigurations;
        this.dialogService = dialogService;
        this.subNavigation = RibbonHelper.CreateRibbonGroup(I18NRibbon.ExImport, false);
        this.modInfo = RibbonHelper.CreateRibbonGroup(I18NExport.HeaderMod, false);
        this.InitSubNavigation();
        this.InitAdditionalInformation();
        this.InitSelectedSessionInfo();
    }

    private void InitSubNavigation() {
        this.subNavExport = RibbonHelper.CreateRibbonToggleButton(I18NRibbon.Export,
                                                                   ImageResources.database_arrow_up,
                                                                   true,
                                                                   this.SubNavExportClickAction);
        this.subNavigation.Items.Add(this.subNavExport);
        //
        //
        this.subNavImport = RibbonHelper.CreateRibbonToggleButton(I18NRibbon.Import,
                                                                   ImageResources.database_arrow_down,
                                                                   false,
                                                                   this.SubNavImportClickAction);
        this.subNavigation.Items.Add(this.subNavImport);
        //
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        viewConfiguration.Tab.Items.Add(this.subNavigation);
    }
    private void InitAdditionalInformation() {
        {
            RibbonButton button = RibbonHelper.CreateRibbonButton(I18NExport.ButtonLabelModReadMe,
                                                                  ImageResources.open,
                                                                  this.OpenModReadMeAction,
                                                                  70);
            this.modInfo.Items.Add(button);
        }
        {
            RibbonButton button = RibbonHelper.CreateRibbonButton(I18NExport.ButtonLabelModLanguagesSupported,
                                                                  ImageResources.open,
                                                                  this.OpenModLanguagesSupportedAction,
                                                                  70);
            this.modInfo.Items.Add(button);
        }
        {
            RibbonButton button = RibbonHelper.CreateRibbonButton(I18NExport.ButtonLabelModChangeLog,
                                                                  ImageResources.open,
                                                                  this.OpenModChangeLogAction,
                                                                  70);
            this.modInfo.Items.Add(button);
        }
        //
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        viewConfiguration.Tab.Items.Add(this.modInfo);
    }

    private void OpenModReadMeAction(object sender, RoutedEventArgs e) {
        string title = "README";
        this.OpenModMarkDown(title);
    }
    private void OpenModLanguagesSupportedAction(object sender, RoutedEventArgs e) {
        string title = "LANGUAGES.SUPPORTED";
        this.OpenModMarkDown(title);
    }
    private void OpenModChangeLogAction(object sender, RoutedEventArgs e) {
        string title = "CHANGELOG";
        this.OpenModMarkDown(title);
    }
    private void OpenModMarkDown(string title) {
        Assembly? assembly = Assembly.GetAssembly(typeof(ModMarkDownViewModel));
        string doc = MarkDownHelper.GetMarkDown(assembly, $"TranslateCS2.ExImport.Assets.{title}.MOD.md");
        IDialogParameters parameters = new DialogParameters() {
            { ModMarkDownViewModel.DocParameterName, doc },
            { ModMarkDownViewModel.TitleParameterName, title}
        };
        this.dialogService.ShowDialog(nameof(ModMarkDownView), parameters, null);
    }
    private void InitSelectedSessionInfo() {
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        CurrentSessionInfo selectedSessionInfoGroup = this.containerProvider.Resolve<CurrentSessionInfo>();
        viewConfiguration.Tab.Items.Add(selectedSessionInfoGroup);
    }

    private void SubNavImportClickAction(object sender, RoutedEventArgs e) {
        this.current = typeof(ImportControl);
        this.subNavExport.IsChecked = !this.subNavExport.IsChecked;
        this.SubNavigate();
    }

    private void SubNavExportClickAction(object sender, RoutedEventArgs e) {
        this.current = typeof(ExportControl);
        this.subNavImport.IsChecked = !this.subNavImport.IsChecked;
        this.SubNavigate();
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext) {
        //
        this.subNavigation.IsEnabled = false;
        this.modInfo.IsEnabled = false;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
        this.subNavigation.IsEnabled = true;
        this.modInfo.IsEnabled = true;
        this.SubNavigate();
    }

    private void SubNavigate() {
        this.modInfo.Visibility = Visibility.Collapsed;
        if (this.current == typeof(ExportControl)) {
            this.modInfo.Visibility = Visibility.Visible;
        }
        this.regionManager.RequestNavigate(AppConfigurationManager.AppExportImportRegion, this.current.Name);
    }

    protected override void OnLoadedCommandAction() { }
}
