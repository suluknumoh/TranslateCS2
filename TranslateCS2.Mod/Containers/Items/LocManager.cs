using System;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class LocManager {
    private readonly IMyLogger logger;
    public ILocManagerProvider Provider { get; }
    public string FallbackLocaleId => this.Provider.FallbackLocaleId;
    public LocManager(IMyLogger logger,
                      ILocManagerProvider locManagerProvider) {
        this.logger = logger;
        this.Provider = locManagerProvider;
    }
    public void FlavorChanged(MyLanguage? language,
                              SystemLanguage systemLanguage,
                              string localeId) {
        try {
            if (language is null) {
                return;
            }
            foreach (Translation flavor in language.Flavors) {
                // INFO: its about hashcode and equals...
                this.TryToRemoveSource(language, flavor);
            }
            if (language.HasFlavor(localeId)) {
                // INFO: its about hashcode and equals...
                Translation flavor = language.GetFlavor(localeId);
                this.TryToAddSource(language, flavor);
            }
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(FlavorChanged), ex, language]);
        }
    }
    public void SetActiveLocale(string locale) {
        this.Provider.SetActiveLocale(locale);
    }

    public bool SupportsLocale(string locale) {
        return this.Provider.SupportsLocale(locale);
    }
    public void ReloadActiveLocale() {
        this.Provider.ReloadActiveLocale();
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
    private void TryToAddSource(MyLanguage language,
                                Translation translationFile,
                                bool reThrow = false) {
        try {
            // INFO: its about hashcode and equals...
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
    private void TryToRemoveSource(MyLanguage language,
                                   Translation translationFile) {
        try {
            // INFO: its about hashcode and equals...
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            this.Provider.RemoveSource(language.Id,
                                       translationFile);
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(TryToRemoveSource), ex, translationFile]);
        }
    }

    public SystemLanguage LocaleIdToSystemLanguage(string locale) {
        return this.Provider.LocaleIdToSystemLanguage(locale);
    }
}
