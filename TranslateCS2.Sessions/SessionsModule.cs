using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

using TranslateCS2.Core;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Properties;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Sessions.Controls;
using TranslateCS2.Sessions.ViewModels;
using TranslateCS2.Sessions.Views;

namespace TranslateCS2.Sessions;
[ModuleDependency(nameof(CoreModule))]
public class SessionsModule : IModule {
    public void OnInitialized(IContainerProvider containerProvider) {
        IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
        IViewConfigurations viewConfigurations = containerProvider.Resolve<IViewConfigurations>();
        ITranslationSessionManager translationSessionManager = containerProvider.Resolve<ITranslationSessionManager>();
        viewConfigurations.Add(IViewConfiguration.Create<SessionManagement, SessionManagementViewModel>(I18NRibbon.Sessions, ImageResources.clock_toolbox, true, false, translationSessionManager));
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
        {
            // configure controls
            containerRegistry.RegisterForNavigation<NewEditSessionControl, NewEditSessionControlContext>(nameof(NewEditSessionControl));
        }
    }
}
