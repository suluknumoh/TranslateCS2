using System;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class LocManager {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly IMyLogger logger;
    public ILocManagerProvider Provider { get; }
    public string FallbackLocaleId => this.Provider.FallbackLocaleId;
    public LocManager(ILocManagerProvider locManagerProvider, IModRuntimeContainer runtimeContainer) {
        this.Provider = locManagerProvider;
        this.runtimeContainer = runtimeContainer;
        this.logger = this.runtimeContainer.Logger;
    }
    public void FlavorChanged(MyLanguage? language,
                              SystemLanguage systemLanguage,
                              string localeId) {
        try {
            if (language is null) {
                return;
            }
            foreach (TranslationFile flavor in language.Flavors) {
                this.TryToRemoveSource(language, flavor);
            }
            if (language.HasFlavor(localeId)) {
                TranslationFile flavor = language.GetFlavor(localeId);
                this.TryToAddSource(language, flavor);
            }
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(FlavorChanged), ex, language]);
        }
    }
    public void TryToAddLocale(MyLanguage language) {
        try {
            this.Provider.AddLocale(language.Id,
                                    language.SystemLanguage,
                                    language.Name);
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(TryToAddLocale), ex, language]);
            this.Provider.RemoveLocale(language.Id);
            throw;
        }
    }
    public void TryToAddSource(MyLanguage language,
                               TranslationFile translationFile,
                               bool reThrow = false) {
        try {
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            this.Provider.AddSource(language.Id,
                                    translationFile);
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(TryToAddSource), ex, translationFile]);
            this.TryToRemoveSource(language, translationFile);
            if (reThrow) {
                throw;
            }
        }
    }
    public void TryToRemoveSource(MyLanguage language,
                                  TranslationFile translationFile) {
        try {
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            this.Provider.RemoveSource(language.Id,
                                       translationFile);
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(TryToRemoveSource), ex, translationFile]);
        }
    }
    public void SetActiveLocale(string locale) {
        this.Provider.SetActiveLocale(locale);
    }

    public bool SupportsLocale(string locale) {
        return this.Provider.SupportsLocale(locale);
    }
}
