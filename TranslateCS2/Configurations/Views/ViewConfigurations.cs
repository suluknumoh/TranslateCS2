using System.Collections.Generic;
using System.Linq;

using Prism.Ioc;

namespace TranslateCS2.Configurations.Views;
internal class ViewConfigurations {
    private readonly List<IViewConfiguration> viewConfigurations = [];
    private readonly IContainerRegistry _containerRegistry;

    public IReadOnlyList<IViewConfiguration> ViewConfigurationList => this.viewConfigurations.AsReadOnly();
    public ViewConfigurations(IContainerRegistry containerRegistry) {
        this._containerRegistry = containerRegistry;
    }
    public void Add(IViewConfiguration configuration) {
        this.viewConfigurations.Add(configuration);
    }
    public void Register(Prism.Regions.IRegionManager regionManager) {
        foreach (IViewConfiguration viewConfiguration in this.ViewConfigurationList.Reverse()) {
            // preregister reverse, so startview is visible first
            regionManager.RegisterViewWithRegion(AppConfigurationManager.AppMainRegion, viewConfiguration.View);
        }
        foreach (IViewConfiguration configuration in this.viewConfigurations) {
            configuration.RegisterForNavigation(this._containerRegistry);
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
}
