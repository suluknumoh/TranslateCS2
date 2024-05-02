using System.Collections.Generic;

using Prism.Ioc;

using TranslateCS2.Core.Translators.Collectors;

namespace TranslateCS2.Core.Translators.Modules;
/// <inheritdoc cref="ITranslatorModule" />
public abstract class ATranslatorModule : ITranslatorModule {
    public List<ITranslator> Translators { get; } = [];
    public void OnInitialized(IContainerProvider containerProvider) {
        ITranslatorCollector translatorCollector = containerProvider.Resolve<ITranslatorCollector>();
        foreach (ITranslator translator in this.Translators) {
            // inits and adds the translator
            translatorCollector.AddTranslator(translator);
        }
    }

    public void RegisterTypes(IContainerRegistry containerRegistry) {
        // no need to register and propagate types
    }
}
