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
    private readonly IContainerProvider _containerProvider;
    private readonly IRegionManager _regionManager;
    private readonly IViewConfigurations _viewConfigurations;
    private readonly IDialogService _dialogService;
    private readonly RibbonGroup _subNavigation;
    private readonly RibbonGroup _modInfo;
    private RibbonToggleButton _subNavExport;
    private RibbonToggleButton _subNavImport;
    private Type _current = typeof(ExportControl);

    public ExImPortViewModel(IContainerProvider containerProvider,
                             IRegionManager regionManager,
                             IViewConfigurations viewConfigurations,
                             IDialogService dialogService) {
        this._containerProvider = containerProvider;
        this._regionManager = regionManager;
        this._viewConfigurations = viewConfigurations;
        this._dialogService = dialogService;
        this._subNavigation = RibbonHelper.CreateRibbonGroup(I18NRibbon.ExImport, false);
        this._modInfo = RibbonHelper.CreateRibbonGroup(I18NExport.HeaderMod, false);
        this.InitSubNavigation();
        this.InitAdditionalInformation();
        this.InitSelectedSessionInfo();
    }

    private void InitSubNavigation() {
        this._subNavExport = RibbonHelper.CreateRibbonToggleButton(I18NRibbon.Export,
                                                                   ImageResources.database_arrow_up,
                                                                   true,
                                                                   this.SubNavExportClickAction);
        this._subNavigation.Items.Add(this._subNavExport);
        //
        //
        this._subNavImport = RibbonHelper.CreateRibbonToggleButton(I18NRibbon.Import,
                                                                   ImageResources.database_arrow_down,
                                                                   false,
                                                                   this.SubNavImportClickAction);
        this._subNavigation.Items.Add(this._subNavImport);
        //
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        viewConfiguration.Tab.Items.Add(this._subNavigation);
    }
    private void InitAdditionalInformation() {
        {
            RibbonButton button = RibbonHelper.CreateRibbonButton(I18NExport.ButtonLabelModReadMe,
                                                                  ImageResources.open,
                                                                  this.OpenModReadMeAction);
            this._modInfo.Items.Add(button);
        }
        {
            RibbonButton button = RibbonHelper.CreateRibbonButton(I18NExport.ButtonLabelModChangeLog,
                                                                  ImageResources.open,
                                                                  this.OpenModChangeLogAction);
            this._modInfo.Items.Add(button);
        }
        //
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        viewConfiguration.Tab.Items.Add(this._modInfo);
    }

    private void OpenModReadMeAction(object sender, RoutedEventArgs e) {
        string title = "README";
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
        this._dialogService.ShowDialog(nameof(ModMarkDownView), parameters, null);
    }
    private void InitSelectedSessionInfo() {
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        CurrentSessionInfo selectedSessionInfoGroup = this._containerProvider.Resolve<CurrentSessionInfo>();
        viewConfiguration.Tab.Items.Add(selectedSessionInfoGroup);
    }

    private void SubNavImportClickAction(object sender, RoutedEventArgs e) {
        this._current = typeof(ImportControl);
        this._subNavExport.IsChecked = !this._subNavExport.IsChecked;
        this.SubNavigate();
    }

    private void SubNavExportClickAction(object sender, RoutedEventArgs e) {
        this._current = typeof(ExportControl);
        this._subNavImport.IsChecked = !this._subNavImport.IsChecked;
        this.SubNavigate();
    }

    public override void OnNavigatedFrom(NavigationContext navigationContext) {
        //
        this._subNavigation.IsEnabled = false;
        this._modInfo.IsEnabled = false;
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
        this._subNavigation.IsEnabled = true;
        this._modInfo.IsEnabled = true;
        this.SubNavigate();
    }

    private void SubNavigate() {
        this._modInfo.Visibility = Visibility.Collapsed;
        if (this._current == typeof(ExportControl)) {
            this._modInfo.Visibility = Visibility.Visible;
        }
        this._regionManager.RequestNavigate(AppConfigurationManager.AppExportImportRegion, this._current.Name);
    }

    protected override void OnLoadedCommandAction() { }
}
