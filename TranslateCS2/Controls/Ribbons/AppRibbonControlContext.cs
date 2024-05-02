﻿using System.Windows;
using System.Windows.Controls.Ribbon;

using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;

namespace TranslateCS2.Controls.Ribbons;

internal class AppRibbonControlContext : BindableBase {
    private readonly IRegionManager _regionManager;
    private readonly IViewConfigurations _viewConfigurations;
    private Ribbon? RibbonBar { get; set; }
    public DelegateCommand<RoutedEventArgs> LoadedCommand { get; }
    public AppRibbonControlContext(IRegionManager regionManager,
                                   IViewConfigurations viewConfigurations) {
        this._regionManager = regionManager;
        this._viewConfigurations = viewConfigurations;
        this.LoadedCommand = new DelegateCommand<RoutedEventArgs>(this.LoadedCommandAction);
    }

    private void LoadedCommandAction(RoutedEventArgs args) {
        if (args.Source is Ribbon ribbon) {
            this.RibbonBar = ribbon;
            this._viewConfigurations.DeActivateRibbon = this.DeActivateRibbon;
            foreach (IViewConfiguration viewConfiguration in this._viewConfigurations.ViewConfigurationList) {
                viewConfiguration.NavToggleButton.Click += this.RibbonNavToggleButtonClicked;
                this.RibbonBar.Items.Add(viewConfiguration.Tab);
            }
        }
    }

    private void DeActivateRibbon(bool activate) {
        if (this.RibbonBar != null) {
            App.Current.Dispatcher.Invoke(() => this.RibbonBar.IsEnabled = activate);
        }
    }

    public void RibbonNavToggleButtonClicked(object sender,
                                             RoutedEventArgs routedEventArgs) {
        if (routedEventArgs.Source is RibbonToggleButton clickedButton) {
            string? target = null;
            foreach (IViewConfiguration viewConfiguration in this._viewConfigurations.ViewConfigurationList) {
                if (viewConfiguration.NavToggleButton == null) {
                    continue;
                }
                viewConfiguration.NavToggleButton.IsChecked = clickedButton.Equals(viewConfiguration.NavToggleButton);
                if (clickedButton.Equals(viewConfiguration.NavToggleButton)) {
                    target = viewConfiguration.NavigationTarget;
                    DeActivateRibbonGroups(viewConfiguration, true);
                } else {
                    DeActivateRibbonGroups(viewConfiguration, false);
                }
            }
            this._regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion,
                                               target);
        }
    }

    private static void DeActivateRibbonGroups(IViewConfiguration viewConfiguration,
                                               bool enable) {
        bool first = true;
        foreach (RibbonGroup item in viewConfiguration.Tab.Items) {
            if (first) {
                first = false;
                continue;
            }
            item.IsEnabled = enable;
        }
    }
}
