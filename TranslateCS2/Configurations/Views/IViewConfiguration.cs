using System;
using System.Windows.Controls.Ribbon;

using Prism.Ioc;

namespace TranslateCS2.Configurations.Views;
internal interface IViewConfiguration {
    RibbonTab Tab { get; }
    RibbonToggleButton NavToggleButton { get; }
    string NavigationTarget { get; }
    Type View { get; }
    Type ViewModel { get; }
    string Name { get; }
    void RegisterForNavigation(IContainerRegistry containerRegistry);
}
