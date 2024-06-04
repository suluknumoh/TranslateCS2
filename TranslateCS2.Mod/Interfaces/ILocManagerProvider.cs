using System;

using Colossal;

using TranslateCS2.Mod.Containers.Items;

using UnityEngine;

namespace TranslateCS2.Mod.Interfaces;
/// <summary>
///     <see cref="Colossal.Localization.LocalizationManager"/>
/// </summary>
internal interface ILocManagerProvider {
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.fallbackLocaleId"/>
    /// </summary>
    string FallbackLocaleId { get; }
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.AddLocale(String, SystemLanguage, String)"/>
    /// </summary>
    void AddLocale(string localeId, SystemLanguage systemLanguage, string localeName);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.AddSource(String, IDictionarySource)"/>
    /// </summary>
    void AddSource(string localeId, IDictionarySource source);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.RemoveLocale(String)"/>
    /// </summary>
    void RemoveLocale(string localeId);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.RemoveSource(String, IDictionarySource)"/>
    /// </summary>
    void RemoveSource(string localeId, TranslationFile source);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.SetActiveLocale(String)"/>
    /// </summary>
    void SetActiveLocale(string localeId);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.SupportsLocale(String)"/>
    /// </summary>
    bool SupportsLocale(string localeId);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.LocaleIdToSystemLanguage(String)"/>
    /// </summary>
    SystemLanguage LocaleIdToSystemLanguage(string localeId);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.ReloadActiveLocale"/>
    /// </summary>
    void ReloadActiveLocale();
}
