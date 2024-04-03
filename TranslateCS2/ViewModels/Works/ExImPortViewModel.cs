﻿using System;
using System.Windows;
using System.Windows.Controls.Ribbon;

using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Configurations.Views;
using TranslateCS2.Controls.Exports;
using TranslateCS2.Controls.Imports;
using TranslateCS2.Helpers;
using TranslateCS2.Models;
using TranslateCS2.Properties;
using TranslateCS2.Properties.I18N;

namespace TranslateCS2.ViewModels.Works;
internal class ExImPortViewModel : ABaseViewModel {
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
            Header = I18NRibbon.ExImport,
            IsEnabled = false
        };
        this.InitSubNavigation();
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
    }

    public override void OnNavigatedTo(NavigationContext navigationContext) {
        this._subNavigation.IsEnabled = true;
        this.SubNavigate();
    }

    private void SubNavigate() {
        this._regionManager.RequestNavigate(AppConfigurationManager.AppExportImportRegion, this._current.Name);
    }

    protected override void OnLoadedCommandAction() { }
}
