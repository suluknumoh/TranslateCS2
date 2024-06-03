using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.Json;

using Game.Settings;
using Game.UI.Menu;

using TranslateCS2.Mod.Containers.Items.Unitys;

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

    public delegate void OnFlavorChangedHandler(MyLanguage? language, SystemLanguage systemLanguage, string localeId);
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
            if (value != null) {
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
            MyFlavorDropDownSettingProperty property = new MyFlavorDropDownSettingProperty(this.runtimeContainer,
                                                                                           language,
                                                                                           this,
                                                                                           propertyName);
            MyFlavorDropDownSettingItemData item = new MyFlavorDropDownSettingItemData(this.runtimeContainer,
                                                                                       property,
                                                                                       this,
                                                                                       language);
            pageData[ModSettings.Section].AddItem(item);
        }
    }
    public void SetFlavor(SystemLanguage systemLanguage, object flavorIdObject) {
        if (flavorIdObject is string flavorId) {
            flavorId = this.GetValueToSet(systemLanguage, flavorId, true);
            this.FlavorsSetted[systemLanguage] = flavorId;
        }
    }
    public string GetSettedFlavor(SystemLanguage systemLanguage) {
        this.FlavorsSetted.TryGetValue(systemLanguage, out string? flavorId);
        flavorId ??= DropDownItems.None;
        return this.GetValueToSet(systemLanguage, flavorId, false);
    }

    private string GetValueToSet(SystemLanguage systemLanguage, string localeIdParameter, bool invoke) {
        string localeId = localeIdParameter;
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        if (language is null) {
            // if localeId is none: language has no flavor with such a locale id
            localeId = DropDownItems.None;
        } else {
            if (!language.HasFlavors
                || !language.HasFlavor(localeId)) {
                // no flavors or not the given ones, generally set to none
                localeId = DropDownItems.None;
            }
            if (!language.IsBuiltIn
                && DropDownItems.None.Equals(localeId, StringComparison.OrdinalIgnoreCase)
                && language.HasFlavors) {
                // non built-in languages should be pre initialized with their first flavor
                localeId = language.Flavors.First().Id;
            }
        }
        if (invoke) {
            OnFlavorChanged?.Invoke(language, systemLanguage, localeId);
        }
        return localeId;
    }

    public void SubscribeOnFlavorChanged(OnFlavorChangedHandler handler) {
        this.OnFlavorChanged += handler;
    }
}
