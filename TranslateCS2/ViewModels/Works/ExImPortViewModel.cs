using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Exports;
using TranslateCS2.Controls.Imports;
using TranslateCS2.Helpers;
using TranslateCS2.Properties;

namespace TranslateCS2.ViewModels.Works;
internal class ExImPortViewModel : BindableBase, INavigationAware {
    private readonly IRegionManager _regionManager;
    private readonly ViewConfigurations _viewConfigurations;
    private readonly RibbonGroup _subNavigation;
    private RibbonToggleButton _subNavExport;
    private RibbonToggleButton _subNavImport;
    private Type _current = typeof(ExportControl);

    public ExImPortViewModel(IRegionManager regionManager,
                             ViewConfigurations viewConfigurations) {
        this._regionManager = regionManager;
        this._viewConfigurations = viewConfigurations;
        this._subNavigation = new RibbonGroup() {
            Header = I18N.StringExImportCap,
            IsEnabled = false
        };
        this.InitSubNavigation();
    }

    private void InitSubNavigation() {
        this._subNavExport = new RibbonToggleButton {
            Label = I18N.StringExport,
            LargeImageSource = ImageHelper.GetBitmapImage(ImageResources.database_arrow_up),
            IsChecked = true
        };
        this._subNavExport.Click += this.SubNavExportClickAction;
        this._subNavigation.Items.Add(this._subNavExport);
        //
        //
        this._subNavImport = new RibbonToggleButton {
            Label = I18N.StringImport,
            LargeImageSource = ImageHelper.GetBitmapImage(ImageResources.database_arrow_down),
            IsChecked = false
        };
        this._subNavImport.Click += this.SubNavImportClickAction;
        this._subNavigation.Items.Add(this._subNavImport);
        //
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<ExImPortViewModel>();
        viewConfiguration.Tab.Items.Add(this._subNavigation);
    }

    private void SubNavImportClickAction(object sender, RoutedEventArgs e) {
        this._current = typeof(ImportControl);
        this._subNavImport.IsChecked = true;
        this._subNavExport.IsChecked = false;
        this.SubNavigate();
    }

    private void SubNavExportClickAction(object sender, RoutedEventArgs e) {
        this._current = typeof(ExportControl);
        this._subNavExport.IsChecked = true;
        this._subNavImport.IsChecked = false;
        this.SubNavigate();
    }

    public bool IsNavigationTarget(NavigationContext navigationContext) {
        return true;
    }

    public void OnNavigatedFrom(NavigationContext navigationContext) {
        //
        this._subNavigation.IsEnabled = false;
    }

    public void OnNavigatedTo(NavigationContext navigationContext) {
        this._subNavigation.IsEnabled = true;
        this.SubNavigate();
    }

    private void SubNavigate() {
        this._regionManager.RequestNavigate(AppConfigurationManager.AppExportImportRegion, this._current.Name);
    }
}
