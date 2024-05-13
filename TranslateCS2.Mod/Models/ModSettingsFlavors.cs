using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.Json;

using Game.Settings;
using Game.UI.Menu;
using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal partial class ModSettings {

    public static string Flavor { get; } = nameof(Flavor);

    public delegate void OnFlavorChangedHandler(MyLanguage? language, SystemLanguage systemLanguage, string localeId);

    public event OnFlavorChangedHandler? OnFlavorChanged;

    private Dictionary<SystemLanguage, string> _FlavorsSetted = [];
    [Include]
    [SettingsUIHidden]
    public Dictionary<SystemLanguage, string> FlavorsSetted {
        get => this._FlavorsSetted;
        set {
            if (value != null) {
                this._FlavorsSetted = value;
                foreach (KeyValuePair<SystemLanguage, string> entry in this._FlavorsSetted) {
                    this.GetValueToSet(entry.Key, entry.Value, true);
                }
            }
        }
    }
    private void AddFlavorsToPageData(AutomaticSettings.SettingPageData pageData) {
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is SystemLanguage.Chinese) {
                continue;
            }
            AutomaticSettings.ManualProperty property = new AutomaticSettings.ManualProperty(this.GetType(), typeof(string), ModSettings.GetFlavorPropertyName(systemLanguage)) {
                canRead = true,
                canWrite = true,
                attributes =
                {
                    (Attribute)new SettingsUIDropdownAttribute(this.GetType(), $"{nameof(GetFlavors)}{systemLanguage}")
                },
                setter = (modSettings, localeId) => this.Setter(systemLanguage, localeId),
                getter = (modSettings) => this.Getter(systemLanguage)
            };
            AutomaticSettings.SettingItemData item = new AutomaticSettings.SettingItemData(AutomaticSettings.WidgetType.StringDropdown, this, property) {
                simpleGroup = FlavorGroup,
                disableAction = () => this.IsDisabled(systemLanguage),
                hideAction = () => this.IsHidden(systemLanguage)
            };
            pageData[Section].AddItem(item);
        }
    }
    public static string GetFlavorPropertyName(SystemLanguage systemLanguage) {
        return $"{Flavor}{systemLanguage}";
    }
    public void Setter(SystemLanguage systemLanguage, object localeIdObject) {
        if (localeIdObject is string localeId) {
            localeId = this.GetValueToSet(systemLanguage, localeId, true);
            this.FlavorsSetted[systemLanguage] = localeId;
        }
    }
    public string Getter(SystemLanguage systemLanguage) {
        this.FlavorsSetted.TryGetValue(systemLanguage, out string? val);
        val ??= DropDownItems.None;
        return this.GetValueToSet(systemLanguage, val, false);
    }

    private bool IsHidden(SystemLanguage systemLanguage) {
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        return language == null || (language != null && (!language.ID.Equals(this.runtimeContainer.IntSetting.locale, StringComparison.OrdinalIgnoreCase)));
    }

    private bool IsDisabled(SystemLanguage systemLanguage) {
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        return language == null || (language != null && (!language.ID.Equals(this.runtimeContainer.IntSetting.locale, StringComparison.OrdinalIgnoreCase) || !language.HasFlavors));
    }

    private string GetValueToSet(SystemLanguage systemLanguage, string localeIdParameter, bool invoke) {
        string localeId = localeIdParameter;
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        if (language == null) {
            // if localeId is none: language has no flavor with such a locale id
            localeId = DropDownItems.None;
        } else {
            if (!language.HasFlavors || !language.HasFlavor(localeId)) {
                // no flavors or not the given ones, generally set to none
                localeId = DropDownItems.None;
            }
            if (!language.IsBuiltIn
                && localeId == DropDownItems.None
                && language.HasFlavors) {
                // non built-in languages should be pre initialized with their first flavor
                localeId = language.Flavors.First().LocaleId;
            }
        }
        if (invoke) {
            OnFlavorChanged?.Invoke(language, systemLanguage, localeId);
        }
        return localeId;
    }








    private DropdownItem<string>[] GetFlavors(SystemLanguage systemLanguage) {
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        List<DropdownItem<string>> flavors = this.runtimeContainer.DropDownItems.GetDefault(true);
        if (language != null) {
            // only builtin and those without flavors may have 'none'
            bool addNone = language.IsBuiltIn || !language.HasFlavors;
            flavors = this.runtimeContainer.DropDownItems.GetDefault(addNone);
            flavors.AddRange(language.GetFlavorDropDownItems());
        }
        return flavors.ToArray();
    }

    #region new codeblocks
    public DropdownItem<string>[] GetFlavorsAfrikaans() {
        return this.GetFlavors(SystemLanguage.Afrikaans);
    }
    public DropdownItem<string>[] GetFlavorsArabic() {
        return this.GetFlavors(SystemLanguage.Arabic);
    }
    public DropdownItem<string>[] GetFlavorsBasque() {
        return this.GetFlavors(SystemLanguage.Basque);
    }
    public DropdownItem<string>[] GetFlavorsBelarusian() {
        return this.GetFlavors(SystemLanguage.Belarusian);
    }
    public DropdownItem<string>[] GetFlavorsBulgarian() {
        return this.GetFlavors(SystemLanguage.Bulgarian);
    }
    public DropdownItem<string>[] GetFlavorsCatalan() {
        return this.GetFlavors(SystemLanguage.Catalan);
    }
    public DropdownItem<string>[] GetFlavorsCzech() {
        return this.GetFlavors(SystemLanguage.Czech);
    }
    public DropdownItem<string>[] GetFlavorsDanish() {
        return this.GetFlavors(SystemLanguage.Danish);
    }
    public DropdownItem<string>[] GetFlavorsDutch() {
        return this.GetFlavors(SystemLanguage.Dutch);
    }
    public DropdownItem<string>[] GetFlavorsEnglish() {
        return this.GetFlavors(SystemLanguage.English);
    }
    public DropdownItem<string>[] GetFlavorsEstonian() {
        return this.GetFlavors(SystemLanguage.Estonian);
    }
    public DropdownItem<string>[] GetFlavorsFaroese() {
        return this.GetFlavors(SystemLanguage.Faroese);
    }
    public DropdownItem<string>[] GetFlavorsFinnish() {
        return this.GetFlavors(SystemLanguage.Finnish);
    }
    public DropdownItem<string>[] GetFlavorsFrench() {
        return this.GetFlavors(SystemLanguage.French);
    }
    public DropdownItem<string>[] GetFlavorsGerman() {
        return this.GetFlavors(SystemLanguage.German);
    }
    public DropdownItem<string>[] GetFlavorsGreek() {
        return this.GetFlavors(SystemLanguage.Greek);
    }
    public DropdownItem<string>[] GetFlavorsHebrew() {
        return this.GetFlavors(SystemLanguage.Hebrew);
    }
    public DropdownItem<string>[] GetFlavorsHungarian() {
        return this.GetFlavors(SystemLanguage.Hungarian);
    }
    public DropdownItem<string>[] GetFlavorsIcelandic() {
        return this.GetFlavors(SystemLanguage.Icelandic);
    }
    public DropdownItem<string>[] GetFlavorsIndonesian() {
        return this.GetFlavors(SystemLanguage.Indonesian);
    }
    public DropdownItem<string>[] GetFlavorsItalian() {
        return this.GetFlavors(SystemLanguage.Italian);
    }
    public DropdownItem<string>[] GetFlavorsJapanese() {
        return this.GetFlavors(SystemLanguage.Japanese);
    }
    public DropdownItem<string>[] GetFlavorsKorean() {
        return this.GetFlavors(SystemLanguage.Korean);
    }
    public DropdownItem<string>[] GetFlavorsLatvian() {
        return this.GetFlavors(SystemLanguage.Latvian);
    }
    public DropdownItem<string>[] GetFlavorsLithuanian() {
        return this.GetFlavors(SystemLanguage.Lithuanian);
    }
    public DropdownItem<string>[] GetFlavorsNorwegian() {
        return this.GetFlavors(SystemLanguage.Norwegian);
    }
    public DropdownItem<string>[] GetFlavorsPolish() {
        return this.GetFlavors(SystemLanguage.Polish);
    }
    public DropdownItem<string>[] GetFlavorsPortuguese() {
        return this.GetFlavors(SystemLanguage.Portuguese);
    }
    public DropdownItem<string>[] GetFlavorsRomanian() {
        return this.GetFlavors(SystemLanguage.Romanian);
    }
    public DropdownItem<string>[] GetFlavorsRussian() {
        return this.GetFlavors(SystemLanguage.Russian);
    }
    public DropdownItem<string>[] GetFlavorsSerboCroatian() {
        return this.GetFlavors(SystemLanguage.SerboCroatian);
    }
    public DropdownItem<string>[] GetFlavorsSlovak() {
        return this.GetFlavors(SystemLanguage.Slovak);
    }
    public DropdownItem<string>[] GetFlavorsSlovenian() {
        return this.GetFlavors(SystemLanguage.Slovenian);
    }
    public DropdownItem<string>[] GetFlavorsSpanish() {
        return this.GetFlavors(SystemLanguage.Spanish);
    }
    public DropdownItem<string>[] GetFlavorsSwedish() {
        return this.GetFlavors(SystemLanguage.Swedish);
    }
    public DropdownItem<string>[] GetFlavorsThai() {
        return this.GetFlavors(SystemLanguage.Thai);
    }
    public DropdownItem<string>[] GetFlavorsTurkish() {
        return this.GetFlavors(SystemLanguage.Turkish);
    }
    public DropdownItem<string>[] GetFlavorsUkrainian() {
        return this.GetFlavors(SystemLanguage.Ukrainian);
    }
    public DropdownItem<string>[] GetFlavorsUnknown() {
        return this.GetFlavors(SystemLanguage.Unknown);
    }
    public DropdownItem<string>[] GetFlavorsVietnamese() {
        return this.GetFlavors(SystemLanguage.Vietnamese);
    }
    public DropdownItem<string>[] GetFlavorsChineseSimplified() {
        return this.GetFlavors(SystemLanguage.ChineseSimplified);
    }
    public DropdownItem<string>[] GetFlavorsChineseTraditional() {
        return this.GetFlavors(SystemLanguage.ChineseTraditional);
    }
    public DropdownItem<string>[] GetFlavorsHindi() {
        return this.GetFlavors(SystemLanguage.Hindi);
    }

    #endregion new codeblocks
}
