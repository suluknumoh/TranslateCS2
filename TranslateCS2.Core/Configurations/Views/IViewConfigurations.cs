using System;
using System.Collections.Generic;

using Prism.Regions;

namespace TranslateCS2.Core.Configurations.Views;
public interface IViewConfigurations {
    IReadOnlyList<IViewConfiguration> ViewConfigurationList { get; }
    Action<bool>? DeActivateRibbon { get; set; }
    void Add(IViewConfiguration configuration);
    void Register(IRegionManager regionManager);
    IViewConfiguration? GetViewConfiguration<VM>();
    IViewConfiguration? GetStartViewConfiguration();
}
