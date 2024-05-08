using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.Json;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal partial class ModSettings {
    public delegate void OnFlavorChangedHandler(MyLanguage? language, SystemLanguage systemLanguage, string localeId);

    public event OnFlavorChangedHandler? OnFlavorChanged;

    private string _FlavorAfrikaans = InitFlavor(SystemLanguage.Afrikaans);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsAfrikaans))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorAfrikaansHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorAfrikaansDisabled))]
    public string FlavorAfrikaans {
        get => this._FlavorAfrikaans;
        set => this._FlavorAfrikaans = this.GetValueToSet(SystemLanguage.Afrikaans, value);
    }
    public DropdownItem<string>[] GetFlavorsAfrikaans() {
        return this.GetFlavors(SystemLanguage.Afrikaans);
    }
    public bool IsFlavorAfrikaansHidden() {
        return this.IsHidden(SystemLanguage.Afrikaans);
    }
    public bool IsFlavorAfrikaansDisabled() {
        return this.IsDisabled(SystemLanguage.Afrikaans);
    }



    private string _FlavorArabic = InitFlavor(SystemLanguage.Arabic);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsArabic))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorArabicHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorArabicDisabled))]
    public string FlavorArabic {
        get => this._FlavorArabic;
        set => this._FlavorArabic = this.GetValueToSet(SystemLanguage.Arabic, value);
    }
    public DropdownItem<string>[] GetFlavorsArabic() {
        return this.GetFlavors(SystemLanguage.Arabic);
    }
    public bool IsFlavorArabicHidden() {
        return this.IsHidden(SystemLanguage.Arabic);
    }
    public bool IsFlavorArabicDisabled() {
        return this.IsDisabled(SystemLanguage.Arabic);
    }



    private string _FlavorBasque = InitFlavor(SystemLanguage.Basque);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBasque))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBasqueHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorBasqueDisabled))]
    public string FlavorBasque {
        get => this._FlavorBasque;
        set => this._FlavorBasque = this.GetValueToSet(SystemLanguage.Basque, value);
    }
    public DropdownItem<string>[] GetFlavorsBasque() {
        return this.GetFlavors(SystemLanguage.Basque);
    }
    public bool IsFlavorBasqueHidden() {
        return this.IsHidden(SystemLanguage.Basque);
    }
    public bool IsFlavorBasqueDisabled() {
        return this.IsDisabled(SystemLanguage.Basque);
    }



    private string _FlavorBelarusian = InitFlavor(SystemLanguage.Belarusian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBelarusian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBelarusianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorBelarusianDisabled))]
    public string FlavorBelarusian {
        get => this._FlavorBelarusian;
        set => this._FlavorBelarusian = this.GetValueToSet(SystemLanguage.Belarusian, value);
    }
    public DropdownItem<string>[] GetFlavorsBelarusian() {
        return this.GetFlavors(SystemLanguage.Belarusian);
    }
    public bool IsFlavorBelarusianHidden() {
        return this.IsHidden(SystemLanguage.Belarusian);
    }
    public bool IsFlavorBelarusianDisabled() {
        return this.IsDisabled(SystemLanguage.Belarusian);
    }



    private string _FlavorBulgarian = InitFlavor(SystemLanguage.Bulgarian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBulgarian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBulgarianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorBulgarianDisabled))]
    public string FlavorBulgarian {
        get => this._FlavorBulgarian;
        set => this._FlavorBulgarian = this.GetValueToSet(SystemLanguage.Bulgarian, value);
    }
    public DropdownItem<string>[] GetFlavorsBulgarian() {
        return this.GetFlavors(SystemLanguage.Bulgarian);
    }
    public bool IsFlavorBulgarianHidden() {
        return this.IsHidden(SystemLanguage.Bulgarian);
    }
    public bool IsFlavorBulgarianDisabled() {
        return this.IsDisabled(SystemLanguage.Bulgarian);
    }



    private string _FlavorCatalan = InitFlavor(SystemLanguage.Catalan);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsCatalan))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorCatalanHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorCatalanDisabled))]
    public string FlavorCatalan {
        get => this._FlavorCatalan;
        set => this._FlavorCatalan = this.GetValueToSet(SystemLanguage.Catalan, value);
    }
    public DropdownItem<string>[] GetFlavorsCatalan() {
        return this.GetFlavors(SystemLanguage.Catalan);
    }
    public bool IsFlavorCatalanHidden() {
        return this.IsHidden(SystemLanguage.Catalan);
    }
    public bool IsFlavorCatalanDisabled() {
        return this.IsDisabled(SystemLanguage.Catalan);
    }



    private string _FlavorCzech = InitFlavor(SystemLanguage.Czech);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsCzech))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorCzechHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorCzechDisabled))]
    public string FlavorCzech {
        get => this._FlavorCzech;
        set => this._FlavorCzech = this.GetValueToSet(SystemLanguage.Czech, value);
    }
    public DropdownItem<string>[] GetFlavorsCzech() {
        return this.GetFlavors(SystemLanguage.Czech);
    }
    public bool IsFlavorCzechHidden() {
        return this.IsHidden(SystemLanguage.Czech);
    }
    public bool IsFlavorCzechDisabled() {
        return this.IsDisabled(SystemLanguage.Czech);
    }



    private string _FlavorDanish = InitFlavor(SystemLanguage.Danish);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsDanish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorDanishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorDanishDisabled))]
    public string FlavorDanish {
        get => this._FlavorDanish;
        set => this._FlavorDanish = this.GetValueToSet(SystemLanguage.Danish, value);
    }
    public DropdownItem<string>[] GetFlavorsDanish() {
        return this.GetFlavors(SystemLanguage.Danish);
    }
    public bool IsFlavorDanishHidden() {
        return this.IsHidden(SystemLanguage.Danish);
    }
    public bool IsFlavorDanishDisabled() {
        return this.IsDisabled(SystemLanguage.Danish);
    }



    private string _FlavorDutch = InitFlavor(SystemLanguage.Dutch);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsDutch))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorDutchHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorDutchDisabled))]
    public string FlavorDutch {
        get => this._FlavorDutch;
        set => this._FlavorDutch = this.GetValueToSet(SystemLanguage.Dutch, value);
    }
    public DropdownItem<string>[] GetFlavorsDutch() {
        return this.GetFlavors(SystemLanguage.Dutch);
    }
    public bool IsFlavorDutchHidden() {
        return this.IsHidden(SystemLanguage.Dutch);
    }
    public bool IsFlavorDutchDisabled() {
        return this.IsDisabled(SystemLanguage.Dutch);
    }



    private string _FlavorEnglish = InitFlavor(SystemLanguage.English);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsEnglish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorEnglishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorEnglishDisabled))]
    public string FlavorEnglish {
        get => this._FlavorEnglish;
        set => this._FlavorEnglish = this.GetValueToSet(SystemLanguage.English, value);
    }
    public DropdownItem<string>[] GetFlavorsEnglish() {
        return this.GetFlavors(SystemLanguage.English);
    }
    public bool IsFlavorEnglishHidden() {
        return this.IsHidden(SystemLanguage.English);
    }
    public bool IsFlavorEnglishDisabled() {
        return this.IsDisabled(SystemLanguage.English);
    }



    private string _FlavorEstonian = InitFlavor(SystemLanguage.Estonian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsEstonian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorEstonianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorEstonianDisabled))]
    public string FlavorEstonian {
        get => this._FlavorEstonian;
        set => this._FlavorEstonian = this.GetValueToSet(SystemLanguage.Estonian, value);
    }
    public DropdownItem<string>[] GetFlavorsEstonian() {
        return this.GetFlavors(SystemLanguage.Estonian);
    }
    public bool IsFlavorEstonianHidden() {
        return this.IsHidden(SystemLanguage.Estonian);
    }
    public bool IsFlavorEstonianDisabled() {
        return this.IsDisabled(SystemLanguage.Estonian);
    }



    private string _FlavorFaroese = InitFlavor(SystemLanguage.Faroese);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFaroese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFaroeseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorFaroeseDisabled))]
    public string FlavorFaroese {
        get => this._FlavorFaroese;
        set => this._FlavorFaroese = this.GetValueToSet(SystemLanguage.Faroese, value);
    }
    public DropdownItem<string>[] GetFlavorsFaroese() {
        return this.GetFlavors(SystemLanguage.Faroese);
    }
    public bool IsFlavorFaroeseHidden() {
        return this.IsHidden(SystemLanguage.Faroese);
    }
    public bool IsFlavorFaroeseDisabled() {
        return this.IsDisabled(SystemLanguage.Faroese);
    }



    private string _FlavorFinnish = InitFlavor(SystemLanguage.Finnish);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFinnish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFinnishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorFinnishDisabled))]
    public string FlavorFinnish {
        get => this._FlavorFinnish;
        set => this._FlavorFinnish = this.GetValueToSet(SystemLanguage.Finnish, value);
    }
    public DropdownItem<string>[] GetFlavorsFinnish() {
        return this.GetFlavors(SystemLanguage.Finnish);
    }
    public bool IsFlavorFinnishHidden() {
        return this.IsHidden(SystemLanguage.Finnish);
    }
    public bool IsFlavorFinnishDisabled() {
        return this.IsDisabled(SystemLanguage.Finnish);
    }



    private string _FlavorFrench = InitFlavor(SystemLanguage.French);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFrench))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFrenchHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorFrenchDisabled))]
    public string FlavorFrench {
        get => this._FlavorFrench;
        set => this._FlavorFrench = this.GetValueToSet(SystemLanguage.French, value);
    }
    public DropdownItem<string>[] GetFlavorsFrench() {
        return this.GetFlavors(SystemLanguage.French);
    }
    public bool IsFlavorFrenchHidden() {
        return this.IsHidden(SystemLanguage.French);
    }
    public bool IsFlavorFrenchDisabled() {
        return this.IsDisabled(SystemLanguage.French);
    }



    private string _FlavorGerman = InitFlavor(SystemLanguage.German);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsGerman))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorGermanHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorGermanDisabled))]
    public string FlavorGerman {
        get => this._FlavorGerman;
        set => this._FlavorGerman = this.GetValueToSet(SystemLanguage.German, value);
    }
    public DropdownItem<string>[] GetFlavorsGerman() {
        return this.GetFlavors(SystemLanguage.German);
    }
    public bool IsFlavorGermanHidden() {
        return this.IsHidden(SystemLanguage.German);
    }
    public bool IsFlavorGermanDisabled() {
        return this.IsDisabled(SystemLanguage.German);
    }



    private string _FlavorGreek = InitFlavor(SystemLanguage.Greek);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsGreek))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorGreekHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorGreekDisabled))]
    public string FlavorGreek {
        get => this._FlavorGreek;
        set => this._FlavorGreek = this.GetValueToSet(SystemLanguage.Greek, value);
    }
    public DropdownItem<string>[] GetFlavorsGreek() {
        return this.GetFlavors(SystemLanguage.Greek);
    }
    public bool IsFlavorGreekHidden() {
        return this.IsHidden(SystemLanguage.Greek);
    }
    public bool IsFlavorGreekDisabled() {
        return this.IsDisabled(SystemLanguage.Greek);
    }



    private string _FlavorHebrew = InitFlavor(SystemLanguage.Hebrew);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHebrew))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHebrewHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorHebrewDisabled))]
    public string FlavorHebrew {
        get => this._FlavorHebrew;
        set => this._FlavorHebrew = this.GetValueToSet(SystemLanguage.Hebrew, value);
    }
    public DropdownItem<string>[] GetFlavorsHebrew() {
        return this.GetFlavors(SystemLanguage.Hebrew);
    }
    public bool IsFlavorHebrewHidden() {
        return this.IsHidden(SystemLanguage.Hebrew);
    }
    public bool IsFlavorHebrewDisabled() {
        return this.IsDisabled(SystemLanguage.Hebrew);
    }



    private string _FlavorHungarian = InitFlavor(SystemLanguage.Hungarian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHungarian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHungarianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorHungarianDisabled))]
    public string FlavorHungarian {
        get => this._FlavorHungarian;
        set => this._FlavorHungarian = this.GetValueToSet(SystemLanguage.Hungarian, value);
    }
    public DropdownItem<string>[] GetFlavorsHungarian() {
        return this.GetFlavors(SystemLanguage.Hungarian);
    }
    public bool IsFlavorHungarianHidden() {
        return this.IsHidden(SystemLanguage.Hungarian);
    }
    public bool IsFlavorHungarianDisabled() {
        return this.IsDisabled(SystemLanguage.Hungarian);
    }



    private string _FlavorIcelandic = InitFlavor(SystemLanguage.Icelandic);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsIcelandic))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorIcelandicHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorIcelandicDisabled))]
    public string FlavorIcelandic {
        get => this._FlavorIcelandic;
        set => this._FlavorIcelandic = this.GetValueToSet(SystemLanguage.Icelandic, value);
    }
    public DropdownItem<string>[] GetFlavorsIcelandic() {
        return this.GetFlavors(SystemLanguage.Icelandic);
    }
    public bool IsFlavorIcelandicHidden() {
        return this.IsHidden(SystemLanguage.Icelandic);
    }
    public bool IsFlavorIcelandicDisabled() {
        return this.IsDisabled(SystemLanguage.Icelandic);
    }



    private string _FlavorIndonesian = InitFlavor(SystemLanguage.Indonesian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsIndonesian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorIndonesianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorIndonesianDisabled))]
    public string FlavorIndonesian {
        get => this._FlavorIndonesian;
        set => this._FlavorIndonesian = this.GetValueToSet(SystemLanguage.Indonesian, value);
    }
    public DropdownItem<string>[] GetFlavorsIndonesian() {
        return this.GetFlavors(SystemLanguage.Indonesian);
    }
    public bool IsFlavorIndonesianHidden() {
        return this.IsHidden(SystemLanguage.Indonesian);
    }
    public bool IsFlavorIndonesianDisabled() {
        return this.IsDisabled(SystemLanguage.Indonesian);
    }



    private string _FlavorItalian = InitFlavor(SystemLanguage.Italian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsItalian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorItalianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorItalianDisabled))]
    public string FlavorItalian {
        get => this._FlavorItalian;
        set => this._FlavorItalian = this.GetValueToSet(SystemLanguage.Italian, value);
    }
    public DropdownItem<string>[] GetFlavorsItalian() {
        return this.GetFlavors(SystemLanguage.Italian);
    }
    public bool IsFlavorItalianHidden() {
        return this.IsHidden(SystemLanguage.Italian);
    }
    public bool IsFlavorItalianDisabled() {
        return this.IsDisabled(SystemLanguage.Italian);
    }



    private string _FlavorJapanese = InitFlavor(SystemLanguage.Japanese);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsJapanese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorJapaneseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorJapaneseDisabled))]
    public string FlavorJapanese {
        get => this._FlavorJapanese;
        set => this._FlavorJapanese = this.GetValueToSet(SystemLanguage.Japanese, value);
    }
    public DropdownItem<string>[] GetFlavorsJapanese() {
        return this.GetFlavors(SystemLanguage.Japanese);
    }
    public bool IsFlavorJapaneseHidden() {
        return this.IsHidden(SystemLanguage.Japanese);
    }
    public bool IsFlavorJapaneseDisabled() {
        return this.IsDisabled(SystemLanguage.Japanese);
    }



    private string _FlavorKorean = InitFlavor(SystemLanguage.Korean);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsKorean))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorKoreanHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorKoreanDisabled))]
    public string FlavorKorean {
        get => this._FlavorKorean;
        set => this._FlavorKorean = this.GetValueToSet(SystemLanguage.Korean, value);
    }
    public DropdownItem<string>[] GetFlavorsKorean() {
        return this.GetFlavors(SystemLanguage.Korean);
    }
    public bool IsFlavorKoreanHidden() {
        return this.IsHidden(SystemLanguage.Korean);
    }
    public bool IsFlavorKoreanDisabled() {
        return this.IsDisabled(SystemLanguage.Korean);
    }



    private string _FlavorLatvian = InitFlavor(SystemLanguage.Latvian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsLatvian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorLatvianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorLatvianDisabled))]
    public string FlavorLatvian {
        get => this._FlavorLatvian;
        set => this._FlavorLatvian = this.GetValueToSet(SystemLanguage.Latvian, value);
    }
    public DropdownItem<string>[] GetFlavorsLatvian() {
        return this.GetFlavors(SystemLanguage.Latvian);
    }
    public bool IsFlavorLatvianHidden() {
        return this.IsHidden(SystemLanguage.Latvian);
    }
    public bool IsFlavorLatvianDisabled() {
        return this.IsDisabled(SystemLanguage.Latvian);
    }



    private string _FlavorLithuanian = InitFlavor(SystemLanguage.Lithuanian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsLithuanian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorLithuanianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorLithuanianDisabled))]
    public string FlavorLithuanian {
        get => this._FlavorLithuanian;
        set => this._FlavorLithuanian = this.GetValueToSet(SystemLanguage.Lithuanian, value);
    }
    public DropdownItem<string>[] GetFlavorsLithuanian() {
        return this.GetFlavors(SystemLanguage.Lithuanian);
    }
    public bool IsFlavorLithuanianHidden() {
        return this.IsHidden(SystemLanguage.Lithuanian);
    }
    public bool IsFlavorLithuanianDisabled() {
        return this.IsDisabled(SystemLanguage.Lithuanian);
    }



    private string _FlavorNorwegian = InitFlavor(SystemLanguage.Norwegian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsNorwegian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorNorwegianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorNorwegianDisabled))]
    public string FlavorNorwegian {
        get => this._FlavorNorwegian;
        set => this._FlavorNorwegian = this.GetValueToSet(SystemLanguage.Norwegian, value);
    }
    public DropdownItem<string>[] GetFlavorsNorwegian() {
        return this.GetFlavors(SystemLanguage.Norwegian);
    }
    public bool IsFlavorNorwegianHidden() {
        return this.IsHidden(SystemLanguage.Norwegian);
    }
    public bool IsFlavorNorwegianDisabled() {
        return this.IsDisabled(SystemLanguage.Norwegian);
    }



    private string _FlavorPolish = InitFlavor(SystemLanguage.Polish);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsPolish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorPolishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorPolishDisabled))]
    public string FlavorPolish {
        get => this._FlavorPolish;
        set => this._FlavorPolish = this.GetValueToSet(SystemLanguage.Polish, value);
    }
    public DropdownItem<string>[] GetFlavorsPolish() {
        return this.GetFlavors(SystemLanguage.Polish);
    }
    public bool IsFlavorPolishHidden() {
        return this.IsHidden(SystemLanguage.Polish);
    }
    public bool IsFlavorPolishDisabled() {
        return this.IsDisabled(SystemLanguage.Polish);
    }



    private string _FlavorPortuguese = InitFlavor(SystemLanguage.Portuguese);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsPortuguese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorPortugueseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorPortugueseDisabled))]
    public string FlavorPortuguese {
        get => this._FlavorPortuguese;
        set => this._FlavorPortuguese = this.GetValueToSet(SystemLanguage.Portuguese, value);
    }
    public DropdownItem<string>[] GetFlavorsPortuguese() {
        return this.GetFlavors(SystemLanguage.Portuguese);
    }
    public bool IsFlavorPortugueseHidden() {
        return this.IsHidden(SystemLanguage.Portuguese);
    }
    public bool IsFlavorPortugueseDisabled() {
        return this.IsDisabled(SystemLanguage.Portuguese);
    }



    private string _FlavorRomanian = InitFlavor(SystemLanguage.Romanian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsRomanian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorRomanianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorRomanianDisabled))]
    public string FlavorRomanian {
        get => this._FlavorRomanian;
        set => this._FlavorRomanian = this.GetValueToSet(SystemLanguage.Romanian, value);
    }
    public DropdownItem<string>[] GetFlavorsRomanian() {
        return this.GetFlavors(SystemLanguage.Romanian);
    }
    public bool IsFlavorRomanianHidden() {
        return this.IsHidden(SystemLanguage.Romanian);
    }
    public bool IsFlavorRomanianDisabled() {
        return this.IsDisabled(SystemLanguage.Romanian);
    }



    private string _FlavorRussian = InitFlavor(SystemLanguage.Russian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsRussian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorRussianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorRussianDisabled))]
    public string FlavorRussian {
        get => this._FlavorRussian;
        set => this._FlavorRussian = this.GetValueToSet(SystemLanguage.Russian, value);
    }
    public DropdownItem<string>[] GetFlavorsRussian() {
        return this.GetFlavors(SystemLanguage.Russian);
    }
    public bool IsFlavorRussianHidden() {
        return this.IsHidden(SystemLanguage.Russian);
    }
    public bool IsFlavorRussianDisabled() {
        return this.IsDisabled(SystemLanguage.Russian);
    }



    private string _FlavorSerboCroatian = InitFlavor(SystemLanguage.SerboCroatian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSerboCroatian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSerboCroatianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSerboCroatianDisabled))]
    public string FlavorSerboCroatian {
        get => this._FlavorSerboCroatian;
        set => this._FlavorSerboCroatian = this.GetValueToSet(SystemLanguage.SerboCroatian, value);
    }
    public DropdownItem<string>[] GetFlavorsSerboCroatian() {
        return this.GetFlavors(SystemLanguage.SerboCroatian);
    }
    public bool IsFlavorSerboCroatianHidden() {
        return this.IsHidden(SystemLanguage.SerboCroatian);
    }
    public bool IsFlavorSerboCroatianDisabled() {
        return this.IsDisabled(SystemLanguage.SerboCroatian);
    }



    private string _FlavorSlovak = InitFlavor(SystemLanguage.Slovak);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSlovak))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSlovakHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSlovakDisabled))]
    public string FlavorSlovak {
        get => this._FlavorSlovak;
        set => this._FlavorSlovak = this.GetValueToSet(SystemLanguage.Slovak, value);
    }
    public DropdownItem<string>[] GetFlavorsSlovak() {
        return this.GetFlavors(SystemLanguage.Slovak);
    }
    public bool IsFlavorSlovakHidden() {
        return this.IsHidden(SystemLanguage.Slovak);
    }
    public bool IsFlavorSlovakDisabled() {
        return this.IsDisabled(SystemLanguage.Slovak);
    }



    private string _FlavorSlovenian = InitFlavor(SystemLanguage.Slovenian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSlovenian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSlovenianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSlovenianDisabled))]
    public string FlavorSlovenian {
        get => this._FlavorSlovenian;
        set => this._FlavorSlovenian = this.GetValueToSet(SystemLanguage.Slovenian, value);
    }
    public DropdownItem<string>[] GetFlavorsSlovenian() {
        return this.GetFlavors(SystemLanguage.Slovenian);
    }
    public bool IsFlavorSlovenianHidden() {
        return this.IsHidden(SystemLanguage.Slovenian);
    }
    public bool IsFlavorSlovenianDisabled() {
        return this.IsDisabled(SystemLanguage.Slovenian);
    }



    private string _FlavorSpanish = InitFlavor(SystemLanguage.Spanish);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSpanish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSpanishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSpanishDisabled))]
    public string FlavorSpanish {
        get => this._FlavorSpanish;
        set => this._FlavorSpanish = this.GetValueToSet(SystemLanguage.Spanish, value);
    }
    public DropdownItem<string>[] GetFlavorsSpanish() {
        return this.GetFlavors(SystemLanguage.Spanish);
    }
    public bool IsFlavorSpanishHidden() {
        return this.IsHidden(SystemLanguage.Spanish);
    }
    public bool IsFlavorSpanishDisabled() {
        return this.IsDisabled(SystemLanguage.Spanish);
    }



    private string _FlavorSwedish = InitFlavor(SystemLanguage.Swedish);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSwedish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSwedishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSwedishDisabled))]
    public string FlavorSwedish {
        get => this._FlavorSwedish;
        set => this._FlavorSwedish = this.GetValueToSet(SystemLanguage.Swedish, value);
    }
    public DropdownItem<string>[] GetFlavorsSwedish() {
        return this.GetFlavors(SystemLanguage.Swedish);
    }
    public bool IsFlavorSwedishHidden() {
        return this.IsHidden(SystemLanguage.Swedish);
    }
    public bool IsFlavorSwedishDisabled() {
        return this.IsDisabled(SystemLanguage.Swedish);
    }



    private string _FlavorThai = InitFlavor(SystemLanguage.Thai);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsThai))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorThaiHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorThaiDisabled))]
    public string FlavorThai {
        get => this._FlavorThai;
        set => this._FlavorThai = this.GetValueToSet(SystemLanguage.Thai, value);
    }
    public DropdownItem<string>[] GetFlavorsThai() {
        return this.GetFlavors(SystemLanguage.Thai);
    }
    public bool IsFlavorThaiHidden() {
        return this.IsHidden(SystemLanguage.Thai);
    }
    public bool IsFlavorThaiDisabled() {
        return this.IsDisabled(SystemLanguage.Thai);
    }



    private string _FlavorTurkish = InitFlavor(SystemLanguage.Turkish);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsTurkish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorTurkishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorTurkishDisabled))]
    public string FlavorTurkish {
        get => this._FlavorTurkish;
        set => this._FlavorTurkish = this.GetValueToSet(SystemLanguage.Turkish, value);
    }
    public DropdownItem<string>[] GetFlavorsTurkish() {
        return this.GetFlavors(SystemLanguage.Turkish);
    }
    public bool IsFlavorTurkishHidden() {
        return this.IsHidden(SystemLanguage.Turkish);
    }
    public bool IsFlavorTurkishDisabled() {
        return this.IsDisabled(SystemLanguage.Turkish);
    }



    private string _FlavorUkrainian = InitFlavor(SystemLanguage.Ukrainian);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsUkrainian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorUkrainianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorUkrainianDisabled))]
    public string FlavorUkrainian {
        get => this._FlavorUkrainian;
        set => this._FlavorUkrainian = this.GetValueToSet(SystemLanguage.Ukrainian, value);
    }
    public DropdownItem<string>[] GetFlavorsUkrainian() {
        return this.GetFlavors(SystemLanguage.Ukrainian);
    }
    public bool IsFlavorUkrainianHidden() {
        return this.IsHidden(SystemLanguage.Ukrainian);
    }
    public bool IsFlavorUkrainianDisabled() {
        return this.IsDisabled(SystemLanguage.Ukrainian);
    }



    private string _FlavorUnknown = InitFlavor(SystemLanguage.Unknown);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsUnknown))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorUnknownHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorUnknownDisabled))]
    public string FlavorUnknown {
        get => this._FlavorUnknown;
        set => this._FlavorUnknown = this.GetValueToSet(SystemLanguage.Unknown, value);
    }
    public DropdownItem<string>[] GetFlavorsUnknown() {
        return this.GetFlavors(SystemLanguage.Unknown);
    }
    public bool IsFlavorUnknownHidden() {
        return this.IsHidden(SystemLanguage.Unknown);
    }
    public bool IsFlavorUnknownDisabled() {
        return this.IsDisabled(SystemLanguage.Unknown);
    }



    private string _FlavorVietnamese = InitFlavor(SystemLanguage.Vietnamese);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsVietnamese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorVietnameseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorVietnameseDisabled))]
    public string FlavorVietnamese {
        get => this._FlavorVietnamese;
        set => this._FlavorVietnamese = this.GetValueToSet(SystemLanguage.Vietnamese, value);
    }
    public DropdownItem<string>[] GetFlavorsVietnamese() {
        return this.GetFlavors(SystemLanguage.Vietnamese);
    }
    public bool IsFlavorVietnameseHidden() {
        return this.IsHidden(SystemLanguage.Vietnamese);
    }
    public bool IsFlavorVietnameseDisabled() {
        return this.IsDisabled(SystemLanguage.Vietnamese);
    }



    private string _FlavorChineseSimplified = InitFlavor(SystemLanguage.ChineseSimplified);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsChineseSimplified))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorChineseSimplifiedHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorChineseSimplifiedDisabled))]
    public string FlavorChineseSimplified {
        get => this._FlavorChineseSimplified;
        set => this._FlavorChineseSimplified = this.GetValueToSet(SystemLanguage.ChineseSimplified, value);
    }
    public DropdownItem<string>[] GetFlavorsChineseSimplified() {
        return this.GetFlavors(SystemLanguage.ChineseSimplified);
    }
    public bool IsFlavorChineseSimplifiedHidden() {
        return this.IsHidden(SystemLanguage.ChineseSimplified);
    }
    public bool IsFlavorChineseSimplifiedDisabled() {
        return this.IsDisabled(SystemLanguage.ChineseSimplified);
    }



    private string _FlavorChineseTraditional = InitFlavor(SystemLanguage.ChineseTraditional);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsChineseTraditional))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorChineseTraditionalHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorChineseTraditionalDisabled))]
    public string FlavorChineseTraditional {
        get => this._FlavorChineseTraditional;
        set => this._FlavorChineseTraditional = this.GetValueToSet(SystemLanguage.ChineseTraditional, value);
    }
    public DropdownItem<string>[] GetFlavorsChineseTraditional() {
        return this.GetFlavors(SystemLanguage.ChineseTraditional);
    }
    public bool IsFlavorChineseTraditionalHidden() {
        return this.IsHidden(SystemLanguage.ChineseTraditional);
    }
    public bool IsFlavorChineseTraditionalDisabled() {
        return this.IsDisabled(SystemLanguage.ChineseTraditional);
    }



    private string _FlavorHindi = InitFlavor(SystemLanguage.Hindi);
    [Include]
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHindi))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHindiHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorHindiDisabled))]
    public string FlavorHindi {
        get => this._FlavorHindi;
        set => this._FlavorHindi = this.GetValueToSet(SystemLanguage.Hindi, value);
    }
    public DropdownItem<string>[] GetFlavorsHindi() {
        return this.GetFlavors(SystemLanguage.Hindi);
    }
    public bool IsFlavorHindiHidden() {
        return this.IsHidden(SystemLanguage.Hindi);
    }
    public bool IsFlavorHindiDisabled() {
        return this.IsDisabled(SystemLanguage.Hindi);
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

    private bool IsHidden(SystemLanguage systemLanguage) {
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        return language == null || (language != null && (!language.ID.Equals(this.runtimeContainer.IntSetting.locale, StringComparison.OrdinalIgnoreCase)));
    }

    private bool IsDisabled(SystemLanguage systemLanguage) {
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        return language == null || (language != null && (!language.ID.Equals(this.runtimeContainer.IntSetting.locale, StringComparison.OrdinalIgnoreCase) || !language.HasFlavors));
    }

    private static string InitFlavor(SystemLanguage systemLanguage) {
        MyLanguage? language = ModRuntimeContainerHandler.Instance.RuntimeContainer.Languages.GetLanguage(systemLanguage);
        if (language == null || language.IsBuiltIn || !language.HasFlavors) {
            return DropDownItems.None;
        }
        return language.Flavors.First().LocaleId;
    }

    private string GetValueToSet(SystemLanguage systemLanguage, string localeIdParameter) {
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
        OnFlavorChanged?.Invoke(language, systemLanguage, localeId);
        return localeId;
    }
}
