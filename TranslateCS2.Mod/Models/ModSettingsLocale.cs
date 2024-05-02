using System;
using System.Collections.Generic;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Properties.I18N;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private readonly Dictionary<string, string> readEntriesDictionary = [];
    public Dictionary<string, string> Dictionary = [];
    private static MyLanguages Languages { get; } = MyLanguages.Instance;
    private readonly ModSettings modSettings;
    public ModSettingsLocale(ModSettings setting) {
        this.modSettings = setting;
        this.modSettings.SettingsLocale = this;
        this.InitDictionary();
    }

    private void InitDictionary() {
        this.AddToDictionary(this.modSettings.GetSettingsLocaleID(), ModConstants.NameSimple, false);
        // INFO: what is it for?
        this.AddToDictionary(this.modSettings.GetOptionTabLocaleID(ModSettings.Section), ModSettings.Section, false);

        // reload-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.ReloadGroup), I18NMod.GroupReloadTitle, true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadLabel, true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)), String.Format(I18NMod.GroupReloadButtonReloadDescription, FileSystemHelper.DataFolder), true);
        this.AddToDictionary(this.modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadWarning, true);

        // generate-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.GenerateGroup), I18NMod.GroupGenerateTitle, true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateLocalizationJson)), I18NMod.GroupGenerateButtonGenerateLabel, true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateLocalizationJson)), String.Format(I18NMod.GroupGenerateButtonGenerateDescription, ModConstants.ModExportKeyValueJsonName, FileSystemHelper.DataFolder), true);

        // flavor-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup), I18NMod.GroupFlavorTitle, true);


        // flavor-group-labels: test
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorAfrikaans)), this.GetLabel(SystemLanguage.Afrikaans), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorArabic)), this.GetLabel(SystemLanguage.Arabic), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBasque)), this.GetLabel(SystemLanguage.Basque), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBelarusian)), this.GetLabel(SystemLanguage.Belarusian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBulgarian)), this.GetLabel(SystemLanguage.Bulgarian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCatalan)), this.GetLabel(SystemLanguage.Catalan), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCzech)), this.GetLabel(SystemLanguage.Czech), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDanish)), this.GetLabel(SystemLanguage.Danish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDutch)), this.GetLabel(SystemLanguage.Dutch), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEnglish)), this.GetLabel(SystemLanguage.English), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEstonian)), this.GetLabel(SystemLanguage.Estonian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFaroese)), this.GetLabel(SystemLanguage.Faroese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFinnish)), this.GetLabel(SystemLanguage.Finnish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFrench)), this.GetLabel(SystemLanguage.French), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGerman)), this.GetLabel(SystemLanguage.German), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGreek)), this.GetLabel(SystemLanguage.Greek), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHebrew)), this.GetLabel(SystemLanguage.Hebrew), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHungarian)), this.GetLabel(SystemLanguage.Hungarian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIcelandic)), this.GetLabel(SystemLanguage.Icelandic), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIndonesian)), this.GetLabel(SystemLanguage.Indonesian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorItalian)), this.GetLabel(SystemLanguage.Italian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorJapanese)), this.GetLabel(SystemLanguage.Japanese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorKorean)), this.GetLabel(SystemLanguage.Korean), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLatvian)), this.GetLabel(SystemLanguage.Latvian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLithuanian)), this.GetLabel(SystemLanguage.Lithuanian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorNorwegian)), this.GetLabel(SystemLanguage.Norwegian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPolish)), this.GetLabel(SystemLanguage.Polish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPortuguese)), this.GetLabel(SystemLanguage.Portuguese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRomanian)), this.GetLabel(SystemLanguage.Romanian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRussian)), this.GetLabel(SystemLanguage.Russian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSerboCroatian)), this.GetLabel(SystemLanguage.SerboCroatian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovak)), this.GetLabel(SystemLanguage.Slovak), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovenian)), this.GetLabel(SystemLanguage.Slovenian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSpanish)), this.GetLabel(SystemLanguage.Spanish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSwedish)), this.GetLabel(SystemLanguage.Swedish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorThai)), this.GetLabel(SystemLanguage.Thai), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorTurkish)), this.GetLabel(SystemLanguage.Turkish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorUkrainian)), this.GetLabel(SystemLanguage.Ukrainian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorVietnamese)), this.GetLabel(SystemLanguage.Vietnamese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseSimplified)), this.GetLabel(SystemLanguage.ChineseSimplified), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseTraditional)), this.GetLabel(SystemLanguage.ChineseTraditional), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHindi)), this.GetLabel(SystemLanguage.Hindi), true);




        // flavor-group-descriptions: test
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorAfrikaans)), this.GetDescription(SystemLanguage.Afrikaans), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorArabic)), this.GetDescription(SystemLanguage.Arabic), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBasque)), this.GetDescription(SystemLanguage.Basque), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBelarusian)), this.GetDescription(SystemLanguage.Belarusian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBulgarian)), this.GetDescription(SystemLanguage.Bulgarian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCatalan)), this.GetDescription(SystemLanguage.Catalan), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCzech)), this.GetDescription(SystemLanguage.Czech), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDanish)), this.GetDescription(SystemLanguage.Danish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDutch)), this.GetDescription(SystemLanguage.Dutch), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEnglish)), this.GetDescription(SystemLanguage.English), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEstonian)), this.GetDescription(SystemLanguage.Estonian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFaroese)), this.GetDescription(SystemLanguage.Faroese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFinnish)), this.GetDescription(SystemLanguage.Finnish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFrench)), this.GetDescription(SystemLanguage.French), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGerman)), this.GetDescription(SystemLanguage.German), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGreek)), this.GetDescription(SystemLanguage.Greek), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHebrew)), this.GetDescription(SystemLanguage.Hebrew), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHungarian)), this.GetDescription(SystemLanguage.Hungarian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIcelandic)), this.GetDescription(SystemLanguage.Icelandic), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIndonesian)), this.GetDescription(SystemLanguage.Indonesian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorItalian)), this.GetDescription(SystemLanguage.Italian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorJapanese)), this.GetDescription(SystemLanguage.Japanese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorKorean)), this.GetDescription(SystemLanguage.Korean), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLatvian)), this.GetDescription(SystemLanguage.Latvian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLithuanian)), this.GetDescription(SystemLanguage.Lithuanian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorNorwegian)), this.GetDescription(SystemLanguage.Norwegian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPolish)), this.GetDescription(SystemLanguage.Polish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPortuguese)), this.GetDescription(SystemLanguage.Portuguese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRomanian)), this.GetDescription(SystemLanguage.Romanian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRussian)), this.GetDescription(SystemLanguage.Russian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSerboCroatian)), this.GetDescription(SystemLanguage.SerboCroatian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovak)), this.GetDescription(SystemLanguage.Slovak), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovenian)), this.GetDescription(SystemLanguage.Slovenian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSpanish)), this.GetDescription(SystemLanguage.Spanish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSwedish)), this.GetDescription(SystemLanguage.Swedish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorThai)), this.GetDescription(SystemLanguage.Thai), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorTurkish)), this.GetDescription(SystemLanguage.Turkish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorUkrainian)), this.GetDescription(SystemLanguage.Ukrainian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorVietnamese)), this.GetDescription(SystemLanguage.Vietnamese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseSimplified)), this.GetDescription(SystemLanguage.ChineseSimplified), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseTraditional)), this.GetDescription(SystemLanguage.ChineseTraditional), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHindi)), this.GetDescription(SystemLanguage.Hindi), true);


    }


    private void AddToDictionary(string key, string value, bool isExportable) {
        this.readEntriesDictionary.Add(key, value);
        if (isExportable) {
            this.Dictionary.Add(key, value);
        }
    }


    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return this.readEntriesDictionary;
    }

    private string GetLabel(SystemLanguage systemLanguage) {
        string param = this.GetLanguageName(systemLanguage);
        string messagePre = I18NMod.FlavorLabel;
        return $"{messagePre} {param}";
    }
    private string GetDescription(SystemLanguage systemLanguage) {
        string param = this.GetLanguageName(systemLanguage);
        string messagePre = I18NMod.FlavorDescription;
        return $"{messagePre} {param}";
    }
    private string GetLanguageName(SystemLanguage systemLanguage) {
        MyLanguage? language = Languages.GetLanguage(systemLanguage);
        if (language == null) {
            return systemLanguage.ToString();
        }
        return language.NameEnglish;
    }

    public void Unload() { }
}
