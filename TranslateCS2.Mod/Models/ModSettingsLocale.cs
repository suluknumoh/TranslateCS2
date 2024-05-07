using System;
using System.Collections.Generic;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Properties.I18N;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private readonly ModRuntimeContainerHandler runtimeContainerHandler;
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly Dictionary<string, string> readEntriesDictionary = [];
    public Dictionary<string, string> Dictionary = [];
    private readonly MyLanguages languages;
    private readonly ModSettings modSettings;
    public ModSettingsLocale(ModSettings setting, ModRuntimeContainerHandler runtimeContainerHandler) {
        this.modSettings = setting;
        this.runtimeContainerHandler = runtimeContainerHandler;
        this.runtimeContainer = runtimeContainerHandler.RuntimeContainer;
        this.modSettings.SettingsLocale = this;
        this.languages = this.runtimeContainer.Languages;
        this.InitDictionary();
    }

    private void InitDictionary() {
        this.AddToDictionary(this.modSettings.GetSettingsLocaleID(), ModConstants.NameSimple, false);
        // INFO: what is it for?
        this.AddToDictionary(this.modSettings.GetOptionTabLocaleID(ModSettings.Section), ModSettings.Section, false);

        // reload-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.ReloadGroup), I18NMod.GroupReloadTitle, true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadLabel, true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)), String.Format(I18NMod.GroupReloadButtonReloadDescription, this.runtimeContainer.Paths.ModsDataPathSpecific), true);
        this.AddToDictionary(this.modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadWarning, true);

        // generate-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.GenerateGroup), I18NMod.GroupGenerateTitle, true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateLocalizationJson)), I18NMod.GroupGenerateButtonGenerateLabel, true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateLocalizationJson)), String.Format(I18NMod.GroupGenerateButtonGenerateDescription, ModConstants.ModExportKeyValueJsonName, this.runtimeContainer.Paths.ModsDataPathSpecific), true);

        // flavor-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup), I18NMod.GroupFlavorTitle, true);


        // flavor-group-labels: test
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorAfrikaans"), this.GetLabel(SystemLanguage.Afrikaans), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorArabic"), this.GetLabel(SystemLanguage.Arabic), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorBasque"), this.GetLabel(SystemLanguage.Basque), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorBelarusian"), this.GetLabel(SystemLanguage.Belarusian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorBulgarian"), this.GetLabel(SystemLanguage.Bulgarian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorCatalan"), this.GetLabel(SystemLanguage.Catalan), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorCzech"), this.GetLabel(SystemLanguage.Czech), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorDanish"), this.GetLabel(SystemLanguage.Danish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorDutch"), this.GetLabel(SystemLanguage.Dutch), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorEnglish"), this.GetLabel(SystemLanguage.English), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorEstonian"), this.GetLabel(SystemLanguage.Estonian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorFaroese"), this.GetLabel(SystemLanguage.Faroese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorFinnish"), this.GetLabel(SystemLanguage.Finnish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorFrench"), this.GetLabel(SystemLanguage.French), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorGerman"), this.GetLabel(SystemLanguage.German), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorGreek"), this.GetLabel(SystemLanguage.Greek), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorHebrew"), this.GetLabel(SystemLanguage.Hebrew), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorHungarian"), this.GetLabel(SystemLanguage.Hungarian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorIcelandic"), this.GetLabel(SystemLanguage.Icelandic), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorIndonesian"), this.GetLabel(SystemLanguage.Indonesian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorItalian"), this.GetLabel(SystemLanguage.Italian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorJapanese"), this.GetLabel(SystemLanguage.Japanese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorKorean"), this.GetLabel(SystemLanguage.Korean), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorLatvian"), this.GetLabel(SystemLanguage.Latvian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorLithuanian"), this.GetLabel(SystemLanguage.Lithuanian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorNorwegian"), this.GetLabel(SystemLanguage.Norwegian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorPolish"), this.GetLabel(SystemLanguage.Polish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorPortuguese"), this.GetLabel(SystemLanguage.Portuguese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorRomanian"), this.GetLabel(SystemLanguage.Romanian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorRussian"), this.GetLabel(SystemLanguage.Russian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorSerboCroatian"), this.GetLabel(SystemLanguage.SerboCroatian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorSlovak"), this.GetLabel(SystemLanguage.Slovak), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorSlovenian"), this.GetLabel(SystemLanguage.Slovenian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorSpanish"), this.GetLabel(SystemLanguage.Spanish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorSwedish"), this.GetLabel(SystemLanguage.Swedish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorThai"), this.GetLabel(SystemLanguage.Thai), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorTurkish"), this.GetLabel(SystemLanguage.Turkish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorUkrainian"), this.GetLabel(SystemLanguage.Ukrainian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorVietnamese"), this.GetLabel(SystemLanguage.Vietnamese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorChineseSimplified"), this.GetLabel(SystemLanguage.ChineseSimplified), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorChineseTraditional"), this.GetLabel(SystemLanguage.ChineseTraditional), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID("FlavorHindi"), this.GetLabel(SystemLanguage.Hindi), true);




        // flavor-group-descriptions: test
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorAfrikaans"), this.GetDescription(SystemLanguage.Afrikaans), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorArabic"), this.GetDescription(SystemLanguage.Arabic), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorBasque"), this.GetDescription(SystemLanguage.Basque), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorBelarusian"), this.GetDescription(SystemLanguage.Belarusian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorBulgarian"), this.GetDescription(SystemLanguage.Bulgarian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorCatalan"), this.GetDescription(SystemLanguage.Catalan), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorCzech"), this.GetDescription(SystemLanguage.Czech), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorDanish"), this.GetDescription(SystemLanguage.Danish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorDutch"), this.GetDescription(SystemLanguage.Dutch), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorEnglish"), this.GetDescription(SystemLanguage.English), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorEstonian"), this.GetDescription(SystemLanguage.Estonian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorFaroese"), this.GetDescription(SystemLanguage.Faroese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorFinnish"), this.GetDescription(SystemLanguage.Finnish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorFrench"), this.GetDescription(SystemLanguage.French), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorGerman"), this.GetDescription(SystemLanguage.German), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorGreek"), this.GetDescription(SystemLanguage.Greek), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorHebrew"), this.GetDescription(SystemLanguage.Hebrew), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorHungarian"), this.GetDescription(SystemLanguage.Hungarian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorIcelandic"), this.GetDescription(SystemLanguage.Icelandic), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorIndonesian"), this.GetDescription(SystemLanguage.Indonesian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorItalian"), this.GetDescription(SystemLanguage.Italian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorJapanese"), this.GetDescription(SystemLanguage.Japanese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorKorean"), this.GetDescription(SystemLanguage.Korean), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorLatvian"), this.GetDescription(SystemLanguage.Latvian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorLithuanian"), this.GetDescription(SystemLanguage.Lithuanian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorNorwegian"), this.GetDescription(SystemLanguage.Norwegian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorPolish"), this.GetDescription(SystemLanguage.Polish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorPortuguese"), this.GetDescription(SystemLanguage.Portuguese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorRomanian"), this.GetDescription(SystemLanguage.Romanian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorRussian"), this.GetDescription(SystemLanguage.Russian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorSerboCroatian"), this.GetDescription(SystemLanguage.SerboCroatian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorSlovak"), this.GetDescription(SystemLanguage.Slovak), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorSlovenian"), this.GetDescription(SystemLanguage.Slovenian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorSpanish"), this.GetDescription(SystemLanguage.Spanish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorSwedish"), this.GetDescription(SystemLanguage.Swedish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorThai"), this.GetDescription(SystemLanguage.Thai), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorTurkish"), this.GetDescription(SystemLanguage.Turkish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorUkrainian"), this.GetDescription(SystemLanguage.Ukrainian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorVietnamese"), this.GetDescription(SystemLanguage.Vietnamese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorChineseSimplified"), this.GetDescription(SystemLanguage.ChineseSimplified), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorChineseTraditional"), this.GetDescription(SystemLanguage.ChineseTraditional), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID("FlavorHindi"), this.GetDescription(SystemLanguage.Hindi), true);


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
        MyLanguage? language = this.languages.GetLanguage(systemLanguage);
        if (language == null) {
            return systemLanguage.ToString();
        }
        return language.NameEnglish;
    }

    public void Unload() { }
}
