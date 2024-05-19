using System;
using System.Collections.Generic;
using System.Linq;

using Prism.Ioc;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;

namespace TranslateCS2.Configurations.Views;
internal class ViewConfigurations : IViewConfigurations {
    private List<IViewConfiguration> viewConfigurations = [];
    private readonly IContainerRegistry containerRegistry;
    private IViewConfiguration? startViewConfiguration;

    public IReadOnlyList<IViewConfiguration> ViewConfigurationList => this.viewConfigurations.AsReadOnly();

    public Action<bool>? DeActivateRibbon { get; set; }

    public ViewConfigurations(IContainerRegistry containerRegistry) {
        this.containerRegistry = containerRegistry;
    }
    public void Add(IViewConfiguration configuration) {
        this.viewConfigurations.Add(configuration);
    }
    public void AddStartViewConfiguration(IViewConfiguration configuration) {
        this.viewConfigurations = this.viewConfigurations.Prepend(configuration).ToList();
        this.startViewConfiguration = configuration;
    }
    public void Register(IRegionManager regionManager) {
        foreach (IViewConfiguration viewConfiguration in this.ViewConfigurationList.Reverse()) {
            // preregister reverse, so startview is visible first
            regionManager.RegisterViewWithRegion(AppConfigurationManager.AppMainRegion, viewConfiguration.View);
        }
        foreach (IViewConfiguration configuration in this.viewConfigurations) {
            configuration.RegisterForNavigation(this.containerRegistry);
        }
    }
    public IViewConfiguration? GetViewConfiguration<VM>() {
        foreach (IViewConfiguration viewConfiguration in this.ViewConfigurationList) {
            if (typeof(VM) == viewConfiguration.ViewModel) {
                return viewConfiguration;
            }
        }
        return null;
    }

    public IViewConfiguration? GetStartViewConfiguration() {
        return this.startViewConfiguration;
    }
}
