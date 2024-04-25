using System.Collections.Generic;

using Game.Settings;
using Game.UI.Widgets;

using TranslateCS2.Mod.Helpers;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal partial class ModSettings {
    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsAfrikaans))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorAfrikaansHidden))]
    public string FlavorAfrikaans { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsAfrikaans() {
        return GetFlavors(SystemLanguage.Afrikaans);
    }
    public bool IsFlavorAfrikaansHidden() {
        return IsHidden(SystemLanguage.Afrikaans);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsArabic))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorArabicHidden))]
    public string FlavorArabic { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsArabic() {
        return GetFlavors(SystemLanguage.Arabic);
    }
    public bool IsFlavorArabicHidden() {
        return IsHidden(SystemLanguage.Arabic);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBasque))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBasqueHidden))]
    public string FlavorBasque { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsBasque() {
        return GetFlavors(SystemLanguage.Basque);
    }
    public bool IsFlavorBasqueHidden() {
        return IsHidden(SystemLanguage.Basque);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBelarusian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBelarusianHidden))]
    public string FlavorBelarusian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsBelarusian() {
        return GetFlavors(SystemLanguage.Belarusian);
    }
    public bool IsFlavorBelarusianHidden() {
        return IsHidden(SystemLanguage.Belarusian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsBulgarian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorBulgarianHidden))]
    public string FlavorBulgarian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsBulgarian() {
        return GetFlavors(SystemLanguage.Bulgarian);
    }
    public bool IsFlavorBulgarianHidden() {
        return IsHidden(SystemLanguage.Bulgarian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsCatalan))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorCatalanHidden))]
    public string FlavorCatalan { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsCatalan() {
        return GetFlavors(SystemLanguage.Catalan);
    }
    public bool IsFlavorCatalanHidden() {
        return IsHidden(SystemLanguage.Catalan);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsCzech))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorCzechHidden))]
    public string FlavorCzech { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsCzech() {
        return GetFlavors(SystemLanguage.Czech);
    }
    public bool IsFlavorCzechHidden() {
        return IsHidden(SystemLanguage.Czech);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsDanish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorDanishHidden))]
    public string FlavorDanish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsDanish() {
        return GetFlavors(SystemLanguage.Danish);
    }
    public bool IsFlavorDanishHidden() {
        return IsHidden(SystemLanguage.Danish);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsDutch))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorDutchHidden))]
    public string FlavorDutch { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsDutch() {
        return GetFlavors(SystemLanguage.Dutch);
    }
    public bool IsFlavorDutchHidden() {
        return IsHidden(SystemLanguage.Dutch);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsEnglish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorEnglishHidden))]
    public string FlavorEnglish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsEnglish() {
        return GetFlavors(SystemLanguage.English);
    }
    public bool IsFlavorEnglishHidden() {
        return IsHidden(SystemLanguage.English);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsEstonian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorEstonianHidden))]
    public string FlavorEstonian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsEstonian() {
        return GetFlavors(SystemLanguage.Estonian);
    }
    public bool IsFlavorEstonianHidden() {
        return IsHidden(SystemLanguage.Estonian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFaroese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFaroeseHidden))]
    public string FlavorFaroese { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsFaroese() {
        return GetFlavors(SystemLanguage.Faroese);
    }
    public bool IsFlavorFaroeseHidden() {
        return IsHidden(SystemLanguage.Faroese);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFinnish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFinnishHidden))]
    public string FlavorFinnish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsFinnish() {
        return GetFlavors(SystemLanguage.Finnish);
    }
    public bool IsFlavorFinnishHidden() {
        return IsHidden(SystemLanguage.Finnish);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsFrench))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorFrenchHidden))]
    public string FlavorFrench { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsFrench() {
        return GetFlavors(SystemLanguage.French);
    }
    public bool IsFlavorFrenchHidden() {
        return IsHidden(SystemLanguage.French);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsGerman))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorGermanHidden))]
    public string FlavorGerman { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsGerman() {
        return GetFlavors(SystemLanguage.German);
    }
    public bool IsFlavorGermanHidden() {
        return IsHidden(SystemLanguage.German);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsGreek))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorGreekHidden))]
    public string FlavorGreek { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsGreek() {
        return GetFlavors(SystemLanguage.Greek);
    }
    public bool IsFlavorGreekHidden() {
        return IsHidden(SystemLanguage.Greek);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHebrew))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHebrewHidden))]
    public string FlavorHebrew { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsHebrew() {
        return GetFlavors(SystemLanguage.Hebrew);
    }
    public bool IsFlavorHebrewHidden() {
        return IsHidden(SystemLanguage.Hebrew);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsIcelandic))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorIcelandicHidden))]
    public string FlavorIcelandic { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsIcelandic() {
        return GetFlavors(SystemLanguage.Icelandic);
    }
    public bool IsFlavorIcelandicHidden() {
        return IsHidden(SystemLanguage.Icelandic);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsIndonesian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorIndonesianHidden))]
    public string FlavorIndonesian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsIndonesian() {
        return GetFlavors(SystemLanguage.Indonesian);
    }
    public bool IsFlavorIndonesianHidden() {
        return IsHidden(SystemLanguage.Indonesian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsItalian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorItalianHidden))]
    public string FlavorItalian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsItalian() {
        return GetFlavors(SystemLanguage.Italian);
    }
    public bool IsFlavorItalianHidden() {
        return IsHidden(SystemLanguage.Italian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsJapanese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorJapaneseHidden))]
    public string FlavorJapanese { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsJapanese() {
        return GetFlavors(SystemLanguage.Japanese);
    }
    public bool IsFlavorJapaneseHidden() {
        return IsHidden(SystemLanguage.Japanese);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsKorean))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorKoreanHidden))]
    public string FlavorKorean { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsKorean() {
        return GetFlavors(SystemLanguage.Korean);
    }
    public bool IsFlavorKoreanHidden() {
        return IsHidden(SystemLanguage.Korean);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsLatvian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorLatvianHidden))]
    public string FlavorLatvian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsLatvian() {
        return GetFlavors(SystemLanguage.Latvian);
    }
    public bool IsFlavorLatvianHidden() {
        return IsHidden(SystemLanguage.Latvian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsLithuanian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorLithuanianHidden))]
    public string FlavorLithuanian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsLithuanian() {
        return GetFlavors(SystemLanguage.Lithuanian);
    }
    public bool IsFlavorLithuanianHidden() {
        return IsHidden(SystemLanguage.Lithuanian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsNorwegian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorNorwegianHidden))]
    public string FlavorNorwegian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsNorwegian() {
        return GetFlavors(SystemLanguage.Norwegian);
    }
    public bool IsFlavorNorwegianHidden() {
        return IsHidden(SystemLanguage.Norwegian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsPolish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorPolishHidden))]
    public string FlavorPolish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsPolish() {
        return GetFlavors(SystemLanguage.Polish);
    }
    public bool IsFlavorPolishHidden() {
        return IsHidden(SystemLanguage.Polish);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsPortuguese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorPortugueseHidden))]
    public string FlavorPortuguese { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsPortuguese() {
        return GetFlavors(SystemLanguage.Portuguese);
    }
    public bool IsFlavorPortugueseHidden() {
        return IsHidden(SystemLanguage.Portuguese);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsRomanian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorRomanianHidden))]
    public string FlavorRomanian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsRomanian() {
        return GetFlavors(SystemLanguage.Romanian);
    }
    public bool IsFlavorRomanianHidden() {
        return IsHidden(SystemLanguage.Romanian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsRussian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorRussianHidden))]
    public string FlavorRussian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsRussian() {
        return GetFlavors(SystemLanguage.Russian);
    }
    public bool IsFlavorRussianHidden() {
        return IsHidden(SystemLanguage.Russian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSerboCroatian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSerboCroatianHidden))]
    public string FlavorSerboCroatian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsSerboCroatian() {
        return GetFlavors(SystemLanguage.SerboCroatian);
    }
    public bool IsFlavorSerboCroatianHidden() {
        return IsHidden(SystemLanguage.SerboCroatian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSlovak))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSlovakHidden))]
    public string FlavorSlovak { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsSlovak() {
        return GetFlavors(SystemLanguage.Slovak);
    }
    public bool IsFlavorSlovakHidden() {
        return IsHidden(SystemLanguage.Slovak);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSlovenian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSlovenianHidden))]
    public string FlavorSlovenian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsSlovenian() {
        return GetFlavors(SystemLanguage.Slovenian);
    }
    public bool IsFlavorSlovenianHidden() {
        return IsHidden(SystemLanguage.Slovenian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSpanish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSpanishHidden))]
    public string FlavorSpanish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsSpanish() {
        return GetFlavors(SystemLanguage.Spanish);
    }
    public bool IsFlavorSpanishHidden() {
        return IsHidden(SystemLanguage.Spanish);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsSwedish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorSwedishHidden))]
    public string FlavorSwedish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsSwedish() {
        return GetFlavors(SystemLanguage.Swedish);
    }
    public bool IsFlavorSwedishHidden() {
        return IsHidden(SystemLanguage.Swedish);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsThai))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorThaiHidden))]
    public string FlavorThai { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsThai() {
        return GetFlavors(SystemLanguage.Thai);
    }
    public bool IsFlavorThaiHidden() {
        return IsHidden(SystemLanguage.Thai);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsTurkish))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorTurkishHidden))]
    public string FlavorTurkish { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsTurkish() {
        return GetFlavors(SystemLanguage.Turkish);
    }
    public bool IsFlavorTurkishHidden() {
        return IsHidden(SystemLanguage.Turkish);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsUkrainian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorUkrainianHidden))]
    public string FlavorUkrainian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsUkrainian() {
        return GetFlavors(SystemLanguage.Ukrainian);
    }
    public bool IsFlavorUkrainianHidden() {
        return IsHidden(SystemLanguage.Ukrainian);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsVietnamese))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorVietnameseHidden))]
    public string FlavorVietnamese { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsVietnamese() {
        return GetFlavors(SystemLanguage.Vietnamese);
    }
    public bool IsFlavorVietnameseHidden() {
        return IsHidden(SystemLanguage.Vietnamese);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsChineseSimplified))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorChineseSimplifiedHidden))]

    public string FlavorChineseSimplified { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsChineseSimplified() {
        return GetFlavors(SystemLanguage.ChineseSimplified);
    }
    public bool IsFlavorChineseSimplifiedHidden() {
        return IsHidden(SystemLanguage.ChineseSimplified);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsChineseTraditional))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorChineseTraditionalHidden))]

    public string FlavorChineseTraditional { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsChineseTraditional() {
        return GetFlavors(SystemLanguage.ChineseTraditional);
    }
    public bool IsFlavorChineseTraditionalHidden() {
        return IsHidden(SystemLanguage.ChineseTraditional);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHindi))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHindiHidden))]
    public string FlavorHindi { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsHindi() {
        return GetFlavors(SystemLanguage.Hindi);
    }
    public bool IsFlavorHindiHidden() {
        return IsHidden(SystemLanguage.Hindi);
    }

    [SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavorsHungarian))]
    [SettingsUISection(Section, FlavorGroup)]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavorHungarianHidden))]
    public string FlavorHungarian { get; set; } = DropDownItemsHelper.None;
    public DropdownItem<string>[] GetFlavorsHungarian() {
        return GetFlavors(SystemLanguage.Hungarian);
    }
    public bool IsFlavorHungarianHidden() {
        return IsHidden(SystemLanguage.Hungarian);
    }

    private static DropdownItem<string>[] GetFlavors(SystemLanguage systemLanguage) {
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        List<DropdownItem<string>> flavors = DropDownItemsHelper.GetDefault();
        if (language != null) {
            flavors.AddRange(language.GetFlavorDropDownItems());
        }
        return flavors.ToArray();
    }

    private static bool IsHidden(SystemLanguage systemLanguage) {
        MyLanguage? language = MyLanguages.Instance.GetLanguage(systemLanguage);
        return language != null && (language.ID != InterfaceSettings.locale || !language.HasFlavors);
    }
}
