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
    public LocManager(ILocManagerProvider locManagerProvider,
                      IModRuntimeContainer runtimeContainer) {
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
    public void SetActiveLocale(string locale) {
        this.Provider.SetActiveLocale(locale);
    }

    public bool SupportsLocale(string locale) {
        return this.Provider.SupportsLocale(locale);
    }

    public void TryToAddLanguageInitially(MyLanguage language) {
        try {
            this.TryToAddLocale(language);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(TryToAddLanguageInitially), ex, language]);
        }
    }
    /// <summary>
    ///     this <see langword="method"/> looks weird
    ///     <br/>
    ///     BUT:
    ///     <br/>
    ///     <see cref="Colossal.Localization.LocalizationManager"/>
    ///     <br/>
    ///     has an <see langword="event"/>
    ///     <br/>
    ///     <see cref="Colossal.Localization.LocalizationManager.onActiveDictionaryChanged"/>
    ///     <br/>
    ///     that can not be raised from outside
    ///     <br/>
    ///     and <see cref="Colossal.Localization.LocalizationManager.NotifyActiveDictionaryChanged"/> is <see langword="private"/>
    ///     <br/>
    ///     <br/>
    ///     what this <see langword="method"/> does,
    ///     <br/>
    ///     it removes the <see cref="TranslationFile"/> and readds it to make <see cref="Colossal.Localization.LocalizationManager"/> call
    ///     <br/>
    ///     <see cref="Colossal.Localization.LocalizationManager.NotifyActiveDictionaryChanged"/>
    ///     <br/>
    ///     and raise
    ///     <see cref="Colossal.Localization.LocalizationManager.onActiveDictionaryChanged"/>
    /// </summary>
    /// <param name="language">
    ///     <see cref="MyLanguage"/>
    /// </param>
    /// <param name="translationFile">
    ///     <see cref="TranslationFile"/>
    /// </param>
    public void ReplaceSource(MyLanguage language,
                              TranslationFile translationFile) {
        // INFO: its about hashcode and equals...
        this.TryToRemoveSource(language,
                               translationFile);
        this.TryToAddSource(language,
                            translationFile);
    }
    private void TryToAddLocale(MyLanguage language) {
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
    private void TryToRemoveSource(MyLanguage language,
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

    public SystemLanguage LocaleIdToSystemLanguage(string locale) {
        return this.Provider.LocaleIdToSystemLanguage(locale);
    }
}
