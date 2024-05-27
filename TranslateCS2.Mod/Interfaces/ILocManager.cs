using Colossal;

using TranslateCS2.Mod.Containers.Items;

using UnityEngine;

namespace TranslateCS2.Mod.Interfaces;
/// <summary>
///     <see cref="Colossal.Localization.LocalizationManager"/>
/// </summary>
internal interface ILocManager {
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.fallbackLocaleId"/>
    /// </summary>
    string FallbackLocaleId { get; }
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.AddLocale(System.String, SystemLanguage, System.String)"/>
    /// </summary>
    void AddLocale(string localeId, SystemLanguage systemLanguage, string localeName);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.AddSource(System.String, IDictionarySource)"/>
    /// </summary>
    void AddSource(string localeId, IDictionarySource source);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.RemoveLocale(System.String)"/>
    /// </summary>
    void RemoveLocale(string localeId);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.RemoveSource(System.String, IDictionarySource)"/>
    /// </summary>
    void RemoveSource(string localeId, TranslationFile source);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.SetActiveLocale(System.String)"/>
    /// </summary>
    void SetActiveLocale(string localeId);
    /// <summary>
    ///     <see cref="Colossal.Localization.LocalizationManager.SupportsLocale(System.String)"/>
    /// </summary>
    bool SupportsLocale(string localeId);
}
