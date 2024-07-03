using System;
using System.Collections.Generic;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class LocManager {
    private readonly IMyLogger logger;
    private readonly Dictionary<string, string> settedFlavors = [];
    public ILocManagerProvider Provider { get; }
    public string FallbackLocaleId => this.Provider.FallbackLocaleId;
    public LocManager(IMyLogger logger,
                      ILocManagerProvider locManagerProvider) {
        this.logger = logger;
        this.Provider = locManagerProvider;
    }
    public void FlavorChanged(MyLanguage? language,
                              string flavorId) {
        try {
            if (language is null) {
                return;
            }
            if (this.settedFlavors.TryGetValue(language.Id,
                                               out string? previousFlavorId)
                && previousFlavorId != null
                && flavorId.Equals(previousFlavorId)) {
                return;
            }

            Flavor? previousFlavor = language.GetFlavor(previousFlavorId);
            if (previousFlavor is not null) {
                // INFO: its about hashcode and equals...
                this.TryToRemoveSource(language, previousFlavor);
            }
            if (language.HasFlavor(flavorId)) {
                // INFO: its about hashcode and equals...
                Flavor flavor = language.GetFlavor(flavorId);
                this.TryToAddSource(language, flavor);
                this.settedFlavors[language.Id] = flavorId;
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
                                Flavor flavor,
                                bool reThrow = false) {
        try {
            // INFO: its about hashcode and equals...
            // has to be languages id, cause the language itself is registered with its own id and the flavor only refers to it
            this.Provider.AddSource(language.Id,
                                    flavor);
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(TryToAddSource), ex, flavor]);
            this.TryToRemoveSource(language, flavor);
            if (reThrow) {
                throw;
            }
        }
    }
    private void TryToRemoveSource(MyLanguage language,
                                   Flavor flavor) {
        try {
            // INFO: its about hashcode and equals...
            // has to be languages id, cause the language itself is registered with its own id and the flavor only refers to it
            this.Provider.RemoveSource(language.Id,
                                       flavor);
        } catch (Exception ex) {
            this.logger.LogError(this.GetType(),
                                 LoggingConstants.FailedTo,
                                 [nameof(TryToRemoveSource), ex, flavor]);
        }
    }

    public SystemLanguage LocaleIdToSystemLanguage(string locale) {
        return this.Provider.LocaleIdToSystemLanguage(locale);
    }
}
