using System.Collections.Generic;
using System.Linq;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Mod.Helpers;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal partial class ModSettings {
    public delegate void OnFlavorChangedHandler(MyLanguage? language, SystemLanguage systemLanguage, string localeId);

    public event OnFlavorChangedHandler? OnFlavorChanged;

    private string _FlavorAfrikaans = InitFlavor(SystemLanguage.Afrikaans);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsAfrikaans))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorAfrikaansHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorAfrikaansDisabled))]
    public string FlavorAfrikaans {
        get => this._FlavorAfrikaans;
        set => this._FlavorAfrikaans = this.GetValueToSet(SystemLanguage.Afrikaans, value);
    }
    public DropdownItem<string>[] GetFlavorsAfrikaans() {
        return GetFlavors(SystemLanguage.Afrikaans);
    }
    public bool IsFlavorAfrikaansHidden() {
        return IsHidden(SystemLanguage.Afrikaans);
    }
    public bool IsFlavorAfrikaansDisabled() {
        return IsDisabled(SystemLanguage.Afrikaans);
    }



    private string _FlavorArabic = InitFlavor(SystemLanguage.Arabic);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsArabic))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorArabicHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorArabicDisabled))]
    public string FlavorArabic {
        get => this._FlavorArabic;
        set => this._FlavorArabic = this.GetValueToSet(SystemLanguage.Arabic, value);
    }
    public DropdownItem<string>[] GetFlavorsArabic() {
        return GetFlavors(SystemLanguage.Arabic);
    }
    public bool IsFlavorArabicHidden() {
        return IsHidden(SystemLanguage.Arabic);
    }
    public bool IsFlavorArabicDisabled() {
        return IsDisabled(SystemLanguage.Arabic);
    }



    private string _FlavorBasque = InitFlavor(SystemLanguage.Basque);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBasque))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBasqueHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorBasqueDisabled))]
    public string FlavorBasque {
        get => this._FlavorBasque;
        set => this._FlavorBasque = this.GetValueToSet(SystemLanguage.Basque, value);
    }
    public DropdownItem<string>[] GetFlavorsBasque() {
        return GetFlavors(SystemLanguage.Basque);
    }
    public bool IsFlavorBasqueHidden() {
        return IsHidden(SystemLanguage.Basque);
    }
    public bool IsFlavorBasqueDisabled() {
        return IsDisabled(SystemLanguage.Basque);
    }



    private string _FlavorBelarusian = InitFlavor(SystemLanguage.Belarusian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBelarusian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBelarusianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorBelarusianDisabled))]
    public string FlavorBelarusian {
        get => this._FlavorBelarusian;
        set => this._FlavorBelarusian = this.GetValueToSet(SystemLanguage.Belarusian, value);
    }
    public DropdownItem<string>[] GetFlavorsBelarusian() {
        return GetFlavors(SystemLanguage.Belarusian);
    }
    public bool IsFlavorBelarusianHidden() {
        return IsHidden(SystemLanguage.Belarusian);
    }
    public bool IsFlavorBelarusianDisabled() {
        return IsDisabled(SystemLanguage.Belarusian);
    }



    private string _FlavorBulgarian = InitFlavor(SystemLanguage.Bulgarian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBulgarian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBulgarianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorBulgarianDisabled))]
    public string FlavorBulgarian {
        get => this._FlavorBulgarian;
        set => this._FlavorBulgarian = this.GetValueToSet(SystemLanguage.Bulgarian, value);
    }
    public DropdownItem<string>[] GetFlavorsBulgarian() {
        return GetFlavors(SystemLanguage.Bulgarian);
    }
    public bool IsFlavorBulgarianHidden() {
        return IsHidden(SystemLanguage.Bulgarian);
    }
    public bool IsFlavorBulgarianDisabled() {
        return IsDisabled(SystemLanguage.Bulgarian);
    }



    private string _FlavorCatalan = InitFlavor(SystemLanguage.Catalan);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsCatalan))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorCatalanHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorCatalanDisabled))]
    public string FlavorCatalan {
        get => this._FlavorCatalan;
        set => this._FlavorCatalan = this.GetValueToSet(SystemLanguage.Catalan, value);
    }
    public DropdownItem<string>[] GetFlavorsCatalan() {
        return GetFlavors(SystemLanguage.Catalan);
    }
    public bool IsFlavorCatalanHidden() {
        return IsHidden(SystemLanguage.Catalan);
    }
    public bool IsFlavorCatalanDisabled() {
        return IsDisabled(SystemLanguage.Catalan);
    }



    private string _FlavorCzech = InitFlavor(SystemLanguage.Czech);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsCzech))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorCzechHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorCzechDisabled))]
    public string FlavorCzech {
        get => this._FlavorCzech;
        set => this._FlavorCzech = this.GetValueToSet(SystemLanguage.Czech, value);
    }
    public DropdownItem<string>[] GetFlavorsCzech() {
        return GetFlavors(SystemLanguage.Czech);
    }
    public bool IsFlavorCzechHidden() {
        return IsHidden(SystemLanguage.Czech);
    }
    public bool IsFlavorCzechDisabled() {
        return IsDisabled(SystemLanguage.Czech);
    }



    private string _FlavorDanish = InitFlavor(SystemLanguage.Danish);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsDanish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorDanishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorDanishDisabled))]
    public string FlavorDanish {
        get => this._FlavorDanish;
        set => this._FlavorDanish = this.GetValueToSet(SystemLanguage.Danish, value);
    }
    public DropdownItem<string>[] GetFlavorsDanish() {
        return GetFlavors(SystemLanguage.Danish);
    }
    public bool IsFlavorDanishHidden() {
        return IsHidden(SystemLanguage.Danish);
    }
    public bool IsFlavorDanishDisabled() {
        return IsDisabled(SystemLanguage.Danish);
    }



    private string _FlavorDutch = InitFlavor(SystemLanguage.Dutch);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsDutch))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorDutchHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorDutchDisabled))]
    public string FlavorDutch {
        get => this._FlavorDutch;
        set => this._FlavorDutch = this.GetValueToSet(SystemLanguage.Dutch, value);
    }
    public DropdownItem<string>[] GetFlavorsDutch() {
        return GetFlavors(SystemLanguage.Dutch);
    }
    public bool IsFlavorDutchHidden() {
        return IsHidden(SystemLanguage.Dutch);
    }
    public bool IsFlavorDutchDisabled() {
        return IsDisabled(SystemLanguage.Dutch);
    }



    private string _FlavorEnglish = InitFlavor(SystemLanguage.English);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsEnglish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorEnglishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorEnglishDisabled))]
    public string FlavorEnglish {
        get => this._FlavorEnglish;
        set => this._FlavorEnglish = this.GetValueToSet(SystemLanguage.English, value);
    }
    public DropdownItem<string>[] GetFlavorsEnglish() {
        return GetFlavors(SystemLanguage.English);
    }
    public bool IsFlavorEnglishHidden() {
        return IsHidden(SystemLanguage.English);
    }
    public bool IsFlavorEnglishDisabled() {
        return IsDisabled(SystemLanguage.English);
    }



    private string _FlavorEstonian = InitFlavor(SystemLanguage.Estonian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsEstonian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorEstonianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorEstonianDisabled))]
    public string FlavorEstonian {
        get => this._FlavorEstonian;
        set => this._FlavorEstonian = this.GetValueToSet(SystemLanguage.Estonian, value);
    }
    public DropdownItem<string>[] GetFlavorsEstonian() {
        return GetFlavors(SystemLanguage.Estonian);
    }
    public bool IsFlavorEstonianHidden() {
        return IsHidden(SystemLanguage.Estonian);
    }
    public bool IsFlavorEstonianDisabled() {
        return IsDisabled(SystemLanguage.Estonian);
    }



    private string _FlavorFaroese = InitFlavor(SystemLanguage.Faroese);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFaroese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFaroeseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorFaroeseDisabled))]
    public string FlavorFaroese {
        get => this._FlavorFaroese;
        set => this._FlavorFaroese = this.GetValueToSet(SystemLanguage.Faroese, value);
    }
    public DropdownItem<string>[] GetFlavorsFaroese() {
        return GetFlavors(SystemLanguage.Faroese);
    }
    public bool IsFlavorFaroeseHidden() {
        return IsHidden(SystemLanguage.Faroese);
    }
    public bool IsFlavorFaroeseDisabled() {
        return IsDisabled(SystemLanguage.Faroese);
    }



    private string _FlavorFinnish = InitFlavor(SystemLanguage.Finnish);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFinnish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFinnishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorFinnishDisabled))]
    public string FlavorFinnish {
        get => this._FlavorFinnish;
        set => this._FlavorFinnish = this.GetValueToSet(SystemLanguage.Finnish, value);
    }
    public DropdownItem<string>[] GetFlavorsFinnish() {
        return GetFlavors(SystemLanguage.Finnish);
    }
    public bool IsFlavorFinnishHidden() {
        return IsHidden(SystemLanguage.Finnish);
    }
    public bool IsFlavorFinnishDisabled() {
        return IsDisabled(SystemLanguage.Finnish);
    }



    private string _FlavorFrench = InitFlavor(SystemLanguage.French);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFrench))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFrenchHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorFrenchDisabled))]
    public string FlavorFrench {
        get => this._FlavorFrench;
        set => this._FlavorFrench = this.GetValueToSet(SystemLanguage.French, value);
    }
    public DropdownItem<string>[] GetFlavorsFrench() {
        return GetFlavors(SystemLanguage.French);
    }
    public bool IsFlavorFrenchHidden() {
        return IsHidden(SystemLanguage.French);
    }
    public bool IsFlavorFrenchDisabled() {
        return IsDisabled(SystemLanguage.French);
    }



    private string _FlavorGerman = InitFlavor(SystemLanguage.German);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsGerman))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorGermanHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorGermanDisabled))]
    public string FlavorGerman {
        get => this._FlavorGerman;
        set => this._FlavorGerman = this.GetValueToSet(SystemLanguage.German, value);
    }
    public DropdownItem<string>[] GetFlavorsGerman() {
        return GetFlavors(SystemLanguage.German);
    }
    public bool IsFlavorGermanHidden() {
        return IsHidden(SystemLanguage.German);
    }
    public bool IsFlavorGermanDisabled() {
        return IsDisabled(SystemLanguage.German);
    }



    private string _FlavorGreek = InitFlavor(SystemLanguage.Greek);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsGreek))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorGreekHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorGreekDisabled))]
    public string FlavorGreek {
        get => this._FlavorGreek;
        set => this._FlavorGreek = this.GetValueToSet(SystemLanguage.Greek, value);
    }
    public DropdownItem<string>[] GetFlavorsGreek() {
        return GetFlavors(SystemLanguage.Greek);
    }
    public bool IsFlavorGreekHidden() {
        return IsHidden(SystemLanguage.Greek);
    }
    public bool IsFlavorGreekDisabled() {
        return IsDisabled(SystemLanguage.Greek);
    }



    private string _FlavorHebrew = InitFlavor(SystemLanguage.Hebrew);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHebrew))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHebrewHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorHebrewDisabled))]
    public string FlavorHebrew {
        get => this._FlavorHebrew;
        set => this._FlavorHebrew = this.GetValueToSet(SystemLanguage.Hebrew, value);
    }
    public DropdownItem<string>[] GetFlavorsHebrew() {
        return GetFlavors(SystemLanguage.Hebrew);
    }
    public bool IsFlavorHebrewHidden() {
        return IsHidden(SystemLanguage.Hebrew);
    }
    public bool IsFlavorHebrewDisabled() {
        return IsDisabled(SystemLanguage.Hebrew);
    }



    private string _FlavorHungarian = InitFlavor(SystemLanguage.Hungarian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHungarian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHungarianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorHungarianDisabled))]
    public string FlavorHungarian {
        get => this._FlavorHungarian;
        set => this._FlavorHungarian = this.GetValueToSet(SystemLanguage.Hungarian, value);
    }
    public DropdownItem<string>[] GetFlavorsHungarian() {
        return GetFlavors(SystemLanguage.Hungarian);
    }
    public bool IsFlavorHungarianHidden() {
        return IsHidden(SystemLanguage.Hungarian);
    }
    public bool IsFlavorHungarianDisabled() {
        return IsDisabled(SystemLanguage.Hungarian);
    }



    private string _FlavorIcelandic = InitFlavor(SystemLanguage.Icelandic);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsIcelandic))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorIcelandicHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorIcelandicDisabled))]
    public string FlavorIcelandic {
        get => this._FlavorIcelandic;
        set => this._FlavorIcelandic = this.GetValueToSet(SystemLanguage.Icelandic, value);
    }
    public DropdownItem<string>[] GetFlavorsIcelandic() {
        return GetFlavors(SystemLanguage.Icelandic);
    }
    public bool IsFlavorIcelandicHidden() {
        return IsHidden(SystemLanguage.Icelandic);
    }
    public bool IsFlavorIcelandicDisabled() {
        return IsDisabled(SystemLanguage.Icelandic);
    }



    private string _FlavorIndonesian = InitFlavor(SystemLanguage.Indonesian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsIndonesian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorIndonesianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorIndonesianDisabled))]
    public string FlavorIndonesian {
        get => this._FlavorIndonesian;
        set => this._FlavorIndonesian = this.GetValueToSet(SystemLanguage.Indonesian, value);
    }
    public DropdownItem<string>[] GetFlavorsIndonesian() {
        return GetFlavors(SystemLanguage.Indonesian);
    }
    public bool IsFlavorIndonesianHidden() {
        return IsHidden(SystemLanguage.Indonesian);
    }
    public bool IsFlavorIndonesianDisabled() {
        return IsDisabled(SystemLanguage.Indonesian);
    }



    private string _FlavorItalian = InitFlavor(SystemLanguage.Italian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsItalian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorItalianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorItalianDisabled))]
    public string FlavorItalian {
        get => this._FlavorItalian;
        set => this._FlavorItalian = this.GetValueToSet(SystemLanguage.Italian, value);
    }
    public DropdownItem<string>[] GetFlavorsItalian() {
        return GetFlavors(SystemLanguage.Italian);
    }
    public bool IsFlavorItalianHidden() {
        return IsHidden(SystemLanguage.Italian);
    }
    public bool IsFlavorItalianDisabled() {
        return IsDisabled(SystemLanguage.Italian);
    }



    private string _FlavorJapanese = InitFlavor(SystemLanguage.Japanese);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsJapanese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorJapaneseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorJapaneseDisabled))]
    public string FlavorJapanese {
        get => this._FlavorJapanese;
        set => this._FlavorJapanese = this.GetValueToSet(SystemLanguage.Japanese, value);
    }
    public DropdownItem<string>[] GetFlavorsJapanese() {
        return GetFlavors(SystemLanguage.Japanese);
    }
    public bool IsFlavorJapaneseHidden() {
        return IsHidden(SystemLanguage.Japanese);
    }
    public bool IsFlavorJapaneseDisabled() {
        return IsDisabled(SystemLanguage.Japanese);
    }



    private string _FlavorKorean = InitFlavor(SystemLanguage.Korean);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsKorean))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorKoreanHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorKoreanDisabled))]
    public string FlavorKorean {
        get => this._FlavorKorean;
        set => this._FlavorKorean = this.GetValueToSet(SystemLanguage.Korean, value);
    }
    public DropdownItem<string>[] GetFlavorsKorean() {
        return GetFlavors(SystemLanguage.Korean);
    }
    public bool IsFlavorKoreanHidden() {
        return IsHidden(SystemLanguage.Korean);
    }
    public bool IsFlavorKoreanDisabled() {
        return IsDisabled(SystemLanguage.Korean);
    }



    private string _FlavorLatvian = InitFlavor(SystemLanguage.Latvian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsLatvian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorLatvianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorLatvianDisabled))]
    public string FlavorLatvian {
        get => this._FlavorLatvian;
        set => this._FlavorLatvian = this.GetValueToSet(SystemLanguage.Latvian, value);
    }
    public DropdownItem<string>[] GetFlavorsLatvian() {
        return GetFlavors(SystemLanguage.Latvian);
    }
    public bool IsFlavorLatvianHidden() {
        return IsHidden(SystemLanguage.Latvian);
    }
    public bool IsFlavorLatvianDisabled() {
        return IsDisabled(SystemLanguage.Latvian);
    }



    private string _FlavorLithuanian = InitFlavor(SystemLanguage.Lithuanian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsLithuanian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorLithuanianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorLithuanianDisabled))]
    public string FlavorLithuanian {
        get => this._FlavorLithuanian;
        set => this._FlavorLithuanian = this.GetValueToSet(SystemLanguage.Lithuanian, value);
    }
    public DropdownItem<string>[] GetFlavorsLithuanian() {
        return GetFlavors(SystemLanguage.Lithuanian);
    }
    public bool IsFlavorLithuanianHidden() {
        return IsHidden(SystemLanguage.Lithuanian);
    }
    public bool IsFlavorLithuanianDisabled() {
        return IsDisabled(SystemLanguage.Lithuanian);
    }



    private string _FlavorNorwegian = InitFlavor(SystemLanguage.Norwegian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsNorwegian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorNorwegianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorNorwegianDisabled))]
    public string FlavorNorwegian {
        get => this._FlavorNorwegian;
        set => this._FlavorNorwegian = this.GetValueToSet(SystemLanguage.Norwegian, value);
    }
    public DropdownItem<string>[] GetFlavorsNorwegian() {
        return GetFlavors(SystemLanguage.Norwegian);
    }
    public bool IsFlavorNorwegianHidden() {
        return IsHidden(SystemLanguage.Norwegian);
    }
    public bool IsFlavorNorwegianDisabled() {
        return IsDisabled(SystemLanguage.Norwegian);
    }



    private string _FlavorPolish = InitFlavor(SystemLanguage.Polish);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsPolish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorPolishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorPolishDisabled))]
    public string FlavorPolish {
        get => this._FlavorPolish;
        set => this._FlavorPolish = this.GetValueToSet(SystemLanguage.Polish, value);
    }
    public DropdownItem<string>[] GetFlavorsPolish() {
        return GetFlavors(SystemLanguage.Polish);
    }
    public bool IsFlavorPolishHidden() {
        return IsHidden(SystemLanguage.Polish);
    }
    public bool IsFlavorPolishDisabled() {
        return IsDisabled(SystemLanguage.Polish);
    }



    private string _FlavorPortuguese = InitFlavor(SystemLanguage.Portuguese);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsPortuguese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorPortugueseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorPortugueseDisabled))]
    public string FlavorPortuguese {
        get => this._FlavorPortuguese;
        set => this._FlavorPortuguese = this.GetValueToSet(SystemLanguage.Portuguese, value);
    }
    public DropdownItem<string>[] GetFlavorsPortuguese() {
        return GetFlavors(SystemLanguage.Portuguese);
    }
    public bool IsFlavorPortugueseHidden() {
        return IsHidden(SystemLanguage.Portuguese);
    }
    public bool IsFlavorPortugueseDisabled() {
        return IsDisabled(SystemLanguage.Portuguese);
    }



    private string _FlavorRomanian = InitFlavor(SystemLanguage.Romanian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsRomanian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorRomanianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorRomanianDisabled))]
    public string FlavorRomanian {
        get => this._FlavorRomanian;
        set => this._FlavorRomanian = this.GetValueToSet(SystemLanguage.Romanian, value);
    }
    public DropdownItem<string>[] GetFlavorsRomanian() {
        return GetFlavors(SystemLanguage.Romanian);
    }
    public bool IsFlavorRomanianHidden() {
        return IsHidden(SystemLanguage.Romanian);
    }
    public bool IsFlavorRomanianDisabled() {
        return IsDisabled(SystemLanguage.Romanian);
    }



    private string _FlavorRussian = InitFlavor(SystemLanguage.Russian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsRussian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorRussianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorRussianDisabled))]
    public string FlavorRussian {
        get => this._FlavorRussian;
        set => this._FlavorRussian = this.GetValueToSet(SystemLanguage.Russian, value);
    }
    public DropdownItem<string>[] GetFlavorsRussian() {
        return GetFlavors(SystemLanguage.Russian);
    }
    public bool IsFlavorRussianHidden() {
        return IsHidden(SystemLanguage.Russian);
    }
    public bool IsFlavorRussianDisabled() {
        return IsDisabled(SystemLanguage.Russian);
    }



    private string _FlavorSerboCroatian = InitFlavor(SystemLanguage.SerboCroatian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSerboCroatian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSerboCroatianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSerboCroatianDisabled))]
    public string FlavorSerboCroatian {
        get => this._FlavorSerboCroatian;
        set => this._FlavorSerboCroatian = this.GetValueToSet(SystemLanguage.SerboCroatian, value);
    }
    public DropdownItem<string>[] GetFlavorsSerboCroatian() {
        return GetFlavors(SystemLanguage.SerboCroatian);
    }
    public bool IsFlavorSerboCroatianHidden() {
        return IsHidden(SystemLanguage.SerboCroatian);
    }
    public bool IsFlavorSerboCroatianDisabled() {
        return IsDisabled(SystemLanguage.SerboCroatian);
    }



    private string _FlavorSlovak = InitFlavor(SystemLanguage.Slovak);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSlovak))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSlovakHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSlovakDisabled))]
    public string FlavorSlovak {
        get => this._FlavorSlovak;
        set => this._FlavorSlovak = this.GetValueToSet(SystemLanguage.Slovak, value);
    }
    public DropdownItem<string>[] GetFlavorsSlovak() {
        return GetFlavors(SystemLanguage.Slovak);
    }
    public bool IsFlavorSlovakHidden() {
        return IsHidden(SystemLanguage.Slovak);
    }
    public bool IsFlavorSlovakDisabled() {
        return IsDisabled(SystemLanguage.Slovak);
    }



    private string _FlavorSlovenian = InitFlavor(SystemLanguage.Slovenian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSlovenian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSlovenianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSlovenianDisabled))]
    public string FlavorSlovenian {
        get => this._FlavorSlovenian;
        set => this._FlavorSlovenian = this.GetValueToSet(SystemLanguage.Slovenian, value);
    }
    public DropdownItem<string>[] GetFlavorsSlovenian() {
        return GetFlavors(SystemLanguage.Slovenian);
    }
    public bool IsFlavorSlovenianHidden() {
        return IsHidden(SystemLanguage.Slovenian);
    }
    public bool IsFlavorSlovenianDisabled() {
        return IsDisabled(SystemLanguage.Slovenian);
    }



    private string _FlavorSpanish = InitFlavor(SystemLanguage.Spanish);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSpanish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSpanishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSpanishDisabled))]
    public string FlavorSpanish {
        get => this._FlavorSpanish;
        set => this._FlavorSpanish = this.GetValueToSet(SystemLanguage.Spanish, value);
    }
    public DropdownItem<string>[] GetFlavorsSpanish() {
        return GetFlavors(SystemLanguage.Spanish);
    }
    public bool IsFlavorSpanishHidden() {
        return IsHidden(SystemLanguage.Spanish);
    }
    public bool IsFlavorSpanishDisabled() {
        return IsDisabled(SystemLanguage.Spanish);
    }



    private string _FlavorSwedish = InitFlavor(SystemLanguage.Swedish);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSwedish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSwedishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorSwedishDisabled))]
    public string FlavorSwedish {
        get => this._FlavorSwedish;
        set => this._FlavorSwedish = this.GetValueToSet(SystemLanguage.Swedish, value);
    }
    public DropdownItem<string>[] GetFlavorsSwedish() {
        return GetFlavors(SystemLanguage.Swedish);
    }
    public bool IsFlavorSwedishHidden() {
        return IsHidden(SystemLanguage.Swedish);
    }
    public bool IsFlavorSwedishDisabled() {
        return IsDisabled(SystemLanguage.Swedish);
    }



    private string _FlavorThai = InitFlavor(SystemLanguage.Thai);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsThai))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorThaiHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorThaiDisabled))]
    public string FlavorThai {
        get => this._FlavorThai;
        set => this._FlavorThai = this.GetValueToSet(SystemLanguage.Thai, value);
    }
    public DropdownItem<string>[] GetFlavorsThai() {
        return GetFlavors(SystemLanguage.Thai);
    }
    public bool IsFlavorThaiHidden() {
        return IsHidden(SystemLanguage.Thai);
    }
    public bool IsFlavorThaiDisabled() {
        return IsDisabled(SystemLanguage.Thai);
    }



    private string _FlavorTurkish = InitFlavor(SystemLanguage.Turkish);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsTurkish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorTurkishHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorTurkishDisabled))]
    public string FlavorTurkish {
        get => this._FlavorTurkish;
        set => this._FlavorTurkish = this.GetValueToSet(SystemLanguage.Turkish, value);
    }
    public DropdownItem<string>[] GetFlavorsTurkish() {
        return GetFlavors(SystemLanguage.Turkish);
    }
    public bool IsFlavorTurkishHidden() {
        return IsHidden(SystemLanguage.Turkish);
    }
    public bool IsFlavorTurkishDisabled() {
        return IsDisabled(SystemLanguage.Turkish);
    }



    private string _FlavorUkrainian = InitFlavor(SystemLanguage.Ukrainian);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsUkrainian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorUkrainianHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorUkrainianDisabled))]
    public string FlavorUkrainian {
        get => this._FlavorUkrainian;
        set => this._FlavorUkrainian = this.GetValueToSet(SystemLanguage.Ukrainian, value);
    }
    public DropdownItem<string>[] GetFlavorsUkrainian() {
        return GetFlavors(SystemLanguage.Ukrainian);
    }
    public bool IsFlavorUkrainianHidden() {
        return IsHidden(SystemLanguage.Ukrainian);
    }
    public bool IsFlavorUkrainianDisabled() {
        return IsDisabled(SystemLanguage.Ukrainian);
    }



    private string _FlavorVietnamese = InitFlavor(SystemLanguage.Vietnamese);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsVietnamese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorVietnameseHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorVietnameseDisabled))]
    public string FlavorVietnamese {
        get => this._FlavorVietnamese;
        set => this._FlavorVietnamese = this.GetValueToSet(SystemLanguage.Vietnamese, value);
    }
    public DropdownItem<string>[] GetFlavorsVietnamese() {
        return GetFlavors(SystemLanguage.Vietnamese);
    }
    public bool IsFlavorVietnameseHidden() {
        return IsHidden(SystemLanguage.Vietnamese);
    }
    public bool IsFlavorVietnameseDisabled() {
        return IsDisabled(SystemLanguage.Vietnamese);
    }



    private string _FlavorChineseSimplified = InitFlavor(SystemLanguage.ChineseSimplified);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsChineseSimplified))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorChineseSimplifiedHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorChineseSimplifiedDisabled))]
    public string FlavorChineseSimplified {
        get => this._FlavorChineseSimplified;
        set => this._FlavorChineseSimplified = this.GetValueToSet(SystemLanguage.ChineseSimplified, value);
    }
    public DropdownItem<string>[] GetFlavorsChineseSimplified() {
        return GetFlavors(SystemLanguage.ChineseSimplified);
    }
    public bool IsFlavorChineseSimplifiedHidden() {
        return IsHidden(SystemLanguage.ChineseSimplified);
    }
    public bool IsFlavorChineseSimplifiedDisabled() {
        return IsDisabled(SystemLanguage.ChineseSimplified);
    }



    private string _FlavorChineseTraditional = InitFlavor(SystemLanguage.ChineseTraditional);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsChineseTraditional))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorChineseTraditionalHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorChineseTraditionalDisabled))]
    public string FlavorChineseTraditional {
        get => this._FlavorChineseTraditional;
        set => this._FlavorChineseTraditional = this.GetValueToSet(SystemLanguage.ChineseTraditional, value);
    }
    public DropdownItem<string>[] GetFlavorsChineseTraditional() {
        return GetFlavors(SystemLanguage.ChineseTraditional);
    }
    public bool IsFlavorChineseTraditionalHidden() {
        return IsHidden(SystemLanguage.ChineseTraditional);
    }
    public bool IsFlavorChineseTraditionalDisabled() {
        return IsDisabled(SystemLanguage.ChineseTraditional);
    }



    private string _FlavorHindi = InitFlavor(SystemLanguage.Hindi);
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHindi))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHindiHidden))]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavorHindiDisabled))]
    public string FlavorHindi {
        get => this._FlavorHindi;
        set => this._FlavorHindi = this.GetValueToSet(SystemLanguage.Hindi, value);
    }
    public DropdownItem<string>[] GetFlavorsHindi() {
        return GetFlavors(SystemLanguage.Hindi);
    }
    public bool IsFlavorHindiHidden() {
        return IsHidden(SystemLanguage.Hindi);
    }
    public bool IsFlavorHindiDisabled() {
        return IsDisabled(SystemLanguage.Hindi);
    }







    private static DropdownItem<string>[] GetFlavors(SystemLanguage systemLanguage) {
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        List<DropdownItem<string>> flavors = DropDownItemsHelper.GetDefault(true);
        if (language != null) {
            // only builtin and those without flavors may have 'none'
            bool addNone = language.IsBuiltIn || !language.HasFlavors;
            flavors = DropDownItemsHelper.GetDefault(addNone);
            flavors.AddRange(language.GetFlavorDropDownItems());
        }
        return flavors.ToArray();
    }

    private static bool IsHidden(SystemLanguage systemLanguage) {
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        return language == null || (language != null && (language.ID != InterfaceSettings.locale));
    }

    private static bool IsDisabled(SystemLanguage systemLanguage) {
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        return language == null || (language != null && (language.ID != InterfaceSettings.locale || !language.HasFlavors));
    }

    private static string InitFlavor(SystemLanguage systemLanguage) {
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        if (language == null || language.IsBuiltIn || !language.HasFlavors) {
            return DropDownItemsHelper.None;
        }
        return language.Flavors.First().LocaleId;
    }

    private string GetValueToSet(SystemLanguage systemLanguage, string localeIdParameter) {
        string localeId = localeIdParameter;
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        if (language == null) {
            // if localeId is none: language has no flavor with such a locale id
            localeId = DropDownItemsHelper.None;
        } else {
            if (!language.HasFlavors || !language.HasFlavor(localeId)) {
                localeId = DropDownItemsHelper.None;
            }
        }
        OnFlavorChanged?.Invoke(language, systemLanguage, localeId);
        return localeId;
    }
}
