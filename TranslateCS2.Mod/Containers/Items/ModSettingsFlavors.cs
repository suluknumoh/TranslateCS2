using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.Json;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Mod.Helpers;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal partial class ModSettings {

    /// <inheritdoc cref="GetValueVersion"/>
    private int ValueVersion { get; set; } = 0;
    /// <summary>
    ///     is used to trigger an update of values
    /// </summary>
    public int GetValueVersion() {
        return this.ValueVersion;
    }

    public delegate void OnFlavorChangedHandler(MyLanguage? language, string flavorId);
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


    private string _CurrentLanguage = String.Empty;
    [Exclude]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIValueVersion(typeof(ModSettings), nameof(GetValueVersion))]
    public string CurrentLanguage => this._CurrentLanguage;

    [Exclude]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorDropDownItems))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorDropDownDisabled))]
    [SettingsUIValueVersion(typeof(ModSettings), nameof(GetValueVersion))]
    public string FlavorDropDown {
        get {
            SystemLanguage systemLanguage = this.runtimeContainer.LocManager.LocaleIdToSystemLanguage(this.Locale);
            return this.GetSettedFlavor(systemLanguage);
        }
        set {
            SystemLanguage systemLanguage = this.runtimeContainer.LocManager.LocaleIdToSystemLanguage(this.Locale);
            this.SetFlavor(systemLanguage, value);
        }
    }
    public DropdownItem<string>[] GetFlavorDropDownItems() {
        SystemLanguage systemLanguage = this.runtimeContainer.LocManager.LocaleIdToSystemLanguage(this.Locale);
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        // only builtin and those without flavors may have 'none'
        bool addNone = language is null || language.IsBuiltIn || !language.HasFlavorsWithSources;
        List<DropdownItem<string>> flavors = DropDownItemsHelper.GetDefault(addNone);
        if (language is not null) {
            flavors.AddRange(language.GetFlavorDropDownItems());
        }
        return flavors.ToArray();
    }
    public bool IsFlavorDropDownDisabled() {
        SystemLanguage systemLanguage = this.runtimeContainer.LocManager.LocaleIdToSystemLanguage(this.Locale);
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        // only builtin and those without flavors may have 'none'
        return language is null || !language.HasFlavorsWithSources;
    }






    public void SetFlavor(SystemLanguage systemLanguage, object flavorIdObject) {
        if (flavorIdObject is string flavorId) {
            flavorId = this.GetValueToSet(systemLanguage, flavorId);
            this.FlavorsSetted[systemLanguage] = flavorId;
            MyLanguage? language = this.languages.GetLanguage(systemLanguage);
            this._CurrentLanguage = language?.Name ?? String.Empty;
            this.ValueVersion++;
            OnFlavorChanged?.Invoke(language, flavorId);
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
            if (!language.HasFlavorsWithSources
                || !language.HasFlavor(localeId)) {
                // no flavors or not the given ones, generally set to none
                localeId = DropDownItemsHelper.None;
            }
            if (!language.IsBuiltIn
                && DropDownItemsHelper.None.Equals(localeId, StringComparison.OrdinalIgnoreCase)
                && language.HasFlavorsWithSources) {
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
