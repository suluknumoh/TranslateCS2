using Prism.Modularity;

using TranslateCS2.Core;
using TranslateCS2.Core.Translators.Modules;

namespace TranslateCS2.TranslatorsExample;

[ModuleDependency(nameof(CoreModule))]
public class TranslatorsExampleModule : ATranslatorModule {
    public TranslatorsExampleModule() {
        this.AddTranslators();
    }

    private void AddTranslators() {
        // this is just an example, there may be more
        // there is no need to realize each translator!!!
        // realize your preferred ones and propagate it here
        this.Translators.Add(new TranslatorDeepL());
        //
        this.Translators.Add(new TranslatorExample("Microsoft-Example", "just an example"));
        this.Translators.Add(new TranslatorExample("DeepL-Example", "just an example"));
        this.Translators.Add(new TranslatorExample("LibreTranslate-Example", "just an example"));
        this.Translators.Add(new TranslatorExample("google-Example", "just an example"));
        this.Translators.Add(new TranslatorExample("other-Example", "just an example"));
    }
}
