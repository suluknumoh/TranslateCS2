using Colossal;
using Colossal.Localization;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
/// <summary>
///     wrapper for <see cref="LocalizationManager"/>
/// </summary>
[MyExcludeFromCoverage]
internal class LocManagerProvider : ILocManagerProvider {
    private readonly LocalizationManager localizationManager;

    public LocManagerProvider(LocalizationManager localizationManager) {
        this.localizationManager = localizationManager;
    }

    public string FallbackLocaleId => this.localizationManager.fallbackLocaleId;

    public void AddLocale(string localeId,
                          SystemLanguage systemLanguage,
                          string localeName) {
        this.localizationManager.AddLocale(localeId,
                                           systemLanguage,
                                           localeName);
    }

    public void AddSource(string localeId,
                          IDictionarySource source) {
        this.localizationManager.AddSource(localeId,
                                           source);
    }

    public SystemLanguage LocaleIdToSystemLanguage(string localeId) {
        return this.localizationManager.LocaleIdToSystemLanguage(localeId);
    }

    public void RemoveLocale(string localeId) {
        this.localizationManager.RemoveLocale(localeId);
    }

    public void RemoveSource(string localeId,
                             TranslationFile source) {
        this.localizationManager.RemoveSource(localeId,
                                              source);
    }

    public void SetActiveLocale(string localeId) {
        this.localizationManager.SetActiveLocale(localeId);
    }

    public bool SupportsLocale(string localeId) {
        return this.localizationManager.SupportsLocale(localeId);
    }
}
