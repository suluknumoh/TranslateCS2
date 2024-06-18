using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.Json;

using Game.Settings;
using Game.UI.Menu;

using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Helpers;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal partial class ModSettings {
    public static string Flavor { get; } = nameof(Flavor);
    /// <returns>
    ///     the name of the language-specific property
    ///     <br/>
    ///     that is generetad by <see cref="AddFlavorsToPageData(AutomaticSettings.SettingPageData)"/>
    ///     <br/>
    ///     <br/>
    ///     "FlavorUnknown", for example
    /// </returns>
    public static string GetFlavorLangPropertyName(SystemLanguage systemLanguage) {
        return $"{Flavor}{systemLanguage}";
    }

    public delegate void OnFlavorChangedHandler(MyLanguage? language, SystemLanguage systemLanguage, string flavorId);
    /// <summary>
    ///     do not subscribe directly
    ///     <br/>
    ///     <br/>
    ///     use <see cref="SubscribeOnFlavorChanged(OnFlavorChangedHandler)"/> to subscribe this <see langword="event"/>
    /// </summary>
    private event OnFlavorChangedHandler? OnFlavorChanged;

    private Dictionary<SystemLanguage, string> _FlavorsSetted = [];
    [Include]
    [SettingsUIHidden]
    public Dictionary<SystemLanguage, string> FlavorsSetted {
        get => this._FlavorsSetted;
        set {
            if (value is not null) {
                this._FlavorsSetted = value;
            }
        }
    }
    public void AddFlavorsToPageData(AutomaticSettings.SettingPageData pageData) {
        Dictionary<SystemLanguage, MyLanguage> languageDictionary = this.runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> languageEntry in languageDictionary) {
            SystemLanguage systemLanguage = languageEntry.Key;
            MyLanguage language = languageEntry.Value;
            string propertyName = GetFlavorLangPropertyName(systemLanguage);
            MyFlavorDropDownSettingItemData item = MyFlavorDropDownSettingItemData.Create(language,
                                                                                          this,
                                                                                          propertyName);
            pageData[ModSettings.Section].AddItem(item);
        }
    }
    public void SetFlavor(SystemLanguage systemLanguage, object flavorIdObject) {
        if (flavorIdObject is string flavorId) {
            flavorId = this.GetValueToSet(systemLanguage, flavorId);
            this.FlavorsSetted[systemLanguage] = flavorId;
            MyLanguage? language = this.languages.GetLanguage(systemLanguage);
            OnFlavorChanged?.Invoke(language, systemLanguage, flavorId);
        }
    }
    public string GetSettedFlavor(SystemLanguage systemLanguage) {
        this.FlavorsSetted.TryGetValue(systemLanguage, out string? flavorId);
        flavorId ??= DropDownItemsHelper.None;
        return this.GetValueToSet(systemLanguage, flavorId);
    }

    private string GetValueToSet(SystemLanguage systemLanguage, string localeIdParameter) {
        string localeId = localeIdParameter;
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        if (language is null) {
            // if localeId is none: language has no flavor with such a locale id
            localeId = DropDownItemsHelper.None;
        } else {
            if (!language.HasFlavors
                || !language.HasFlavor(localeId)) {
                // no flavors or not the given ones, generally set to none
                localeId = DropDownItemsHelper.None;
            }
            if (!language.IsBuiltIn
                && DropDownItemsHelper.None.Equals(localeId, StringComparison.OrdinalIgnoreCase)
                && language.HasFlavors) {
                // non built-in languages should be pre initialized with their first flavor
                localeId = language.Flavors.First().Id;
            }
        }
        return localeId;
    }

    public void SubscribeOnFlavorChanged(OnFlavorChangedHandler handler) {
        this.OnFlavorChanged += handler;
    }
}
