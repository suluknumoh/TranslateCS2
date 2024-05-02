using Prism.Ioc;
using Prism.Modularity;

using TranslateCS2.Core;

namespace TranslateCS2.TextSearch;
[ModuleDependency(nameof(CoreModule))]
public class TextSearchModule : IModule {
    public void OnInitialized(IContainerProvider containerProvider) {
        //
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
        // no need to register types
    }
}
