using System.Net.Http;

using Prism.Ioc;
using Prism.Modularity;

using TranslateCS2.Core.HttpClients;
using TranslateCS2.Core.Ribbons.Sessions;
using TranslateCS2.Core.Services.Filters;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Core.Services.LatestVersions;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Core.Translators.Collectors;
using TranslateCS2.Inf.Interfaces;
using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.Core;
public class CoreModule : IModule {
    public void OnInitialized(IContainerProvider containerProvider) {
        //
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
        containerRegistry.RegisterSingleton<HttpClient, AppHttpClient>();
        containerRegistry.RegisterSingleton<IFiltersService, FiltersService>();
        containerRegistry.RegisterSingleton<ILatestVersionCheckService, LatestVersionCheckService>();
        containerRegistry.RegisterSingleton<IInstallPathDetector, InstallPathDetector>();
        containerRegistry.RegisterSingleton<IStreamingDatasDataPathProvider>(containerProvider => containerProvider.Resolve<IInstallPathDetector>());
        containerRegistry.RegisterSingleton<LocFileService>();

        containerRegistry.RegisterSingleton<ITranslationSessionManager, TranslationSessionManager>();
        containerRegistry.RegisterSingleton<ITranslatorCollector, TranslatorCollector>();
        //
        containerRegistry.RegisterForNavigation<CurrentSessionInfo, CurrentSessionInfoContext>();
    }
}
