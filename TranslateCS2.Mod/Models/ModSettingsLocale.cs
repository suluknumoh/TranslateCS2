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
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.LogMarkdownAndCultureInfoNames)), I18NMod.GroupGenerateButtonLogMarkdownAndCultureInfoNamesLabel, true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.LogMarkdownAndCultureInfoNames)), I18NMod.GroupGenerateButtonLogMarkdownAndCultureInfoNamesDescription, true);
        //
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateLocalizationJson)), I18NMod.GroupGenerateButtonGenerateLabel, true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateLocalizationJson)), String.Format(I18NMod.GroupGenerateButtonGenerateDescription, ModConstants.ModExportKeyValueJsonName, this.runtimeContainer.Paths.ModsDataPathSpecific), true);

        // flavor-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup), I18NMod.GroupFlavorTitle, true);


        // flavor-group-labels: test
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Afrikaans}"), this.GetLabel(SystemLanguage.Afrikaans), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Arabic}"), this.GetLabel(SystemLanguage.Arabic), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Basque}"), this.GetLabel(SystemLanguage.Basque), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Belarusian}"), this.GetLabel(SystemLanguage.Belarusian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Bulgarian}"), this.GetLabel(SystemLanguage.Bulgarian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Catalan}"), this.GetLabel(SystemLanguage.Catalan), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Czech}"), this.GetLabel(SystemLanguage.Czech), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Danish}"), this.GetLabel(SystemLanguage.Danish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Dutch}"), this.GetLabel(SystemLanguage.Dutch), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.English}"), this.GetLabel(SystemLanguage.English), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Estonian}"), this.GetLabel(SystemLanguage.Estonian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Faroese}"), this.GetLabel(SystemLanguage.Faroese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Finnish}"), this.GetLabel(SystemLanguage.Finnish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.French}"), this.GetLabel(SystemLanguage.French), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.German}"), this.GetLabel(SystemLanguage.German), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Greek}"), this.GetLabel(SystemLanguage.Greek), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Hebrew}"), this.GetLabel(SystemLanguage.Hebrew), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Hungarian}"), this.GetLabel(SystemLanguage.Hungarian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Icelandic}"), this.GetLabel(SystemLanguage.Icelandic), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Indonesian}"), this.GetLabel(SystemLanguage.Indonesian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Italian}"), this.GetLabel(SystemLanguage.Italian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Japanese}"), this.GetLabel(SystemLanguage.Japanese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Korean}"), this.GetLabel(SystemLanguage.Korean), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Latvian}"), this.GetLabel(SystemLanguage.Latvian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Lithuanian}"), this.GetLabel(SystemLanguage.Lithuanian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Norwegian}"), this.GetLabel(SystemLanguage.Norwegian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Polish}"), this.GetLabel(SystemLanguage.Polish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Portuguese}"), this.GetLabel(SystemLanguage.Portuguese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Romanian}"), this.GetLabel(SystemLanguage.Romanian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Russian}"), this.GetLabel(SystemLanguage.Russian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.SerboCroatian}"), this.GetLabel(SystemLanguage.SerboCroatian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Slovak}"), this.GetLabel(SystemLanguage.Slovak), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Slovenian}"), this.GetLabel(SystemLanguage.Slovenian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Spanish}"), this.GetLabel(SystemLanguage.Spanish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Swedish}"), this.GetLabel(SystemLanguage.Swedish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Thai}"), this.GetLabel(SystemLanguage.Thai), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Turkish}"), this.GetLabel(SystemLanguage.Turkish), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Ukrainian}"), this.GetLabel(SystemLanguage.Ukrainian), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Vietnamese}"), this.GetLabel(SystemLanguage.Vietnamese), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.ChineseSimplified}"), this.GetLabel(SystemLanguage.ChineseSimplified), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.ChineseTraditional}"), this.GetLabel(SystemLanguage.ChineseTraditional), true);
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Hindi}"), this.GetLabel(SystemLanguage.Hindi), true);
        //
        // should be different to other langs logic
        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID($"{ModSettings.Flavor}{SystemLanguage.Unknown}"), LangConstants.OtherLanguages, true);



        // flavor-group-descriptions: test
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Afrikaans}"), this.GetDescription(SystemLanguage.Afrikaans), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Arabic}"), this.GetDescription(SystemLanguage.Arabic), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Basque}"), this.GetDescription(SystemLanguage.Basque), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Belarusian}"), this.GetDescription(SystemLanguage.Belarusian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Bulgarian}"), this.GetDescription(SystemLanguage.Bulgarian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Catalan}"), this.GetDescription(SystemLanguage.Catalan), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Czech}"), this.GetDescription(SystemLanguage.Czech), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Danish}"), this.GetDescription(SystemLanguage.Danish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Dutch}"), this.GetDescription(SystemLanguage.Dutch), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.English}"), this.GetDescription(SystemLanguage.English), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Estonian}"), this.GetDescription(SystemLanguage.Estonian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Faroese}"), this.GetDescription(SystemLanguage.Faroese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Finnish}"), this.GetDescription(SystemLanguage.Finnish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.French}"), this.GetDescription(SystemLanguage.French), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.German}"), this.GetDescription(SystemLanguage.German), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Greek}"), this.GetDescription(SystemLanguage.Greek), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Hebrew}"), this.GetDescription(SystemLanguage.Hebrew), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Hungarian}"), this.GetDescription(SystemLanguage.Hungarian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Icelandic}"), this.GetDescription(SystemLanguage.Icelandic), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Indonesian}"), this.GetDescription(SystemLanguage.Indonesian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Italian}"), this.GetDescription(SystemLanguage.Italian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Japanese}"), this.GetDescription(SystemLanguage.Japanese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Korean}"), this.GetDescription(SystemLanguage.Korean), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Latvian}"), this.GetDescription(SystemLanguage.Latvian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Lithuanian}"), this.GetDescription(SystemLanguage.Lithuanian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Norwegian}"), this.GetDescription(SystemLanguage.Norwegian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Polish}"), this.GetDescription(SystemLanguage.Polish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Portuguese}"), this.GetDescription(SystemLanguage.Portuguese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Romanian}"), this.GetDescription(SystemLanguage.Romanian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Russian}"), this.GetDescription(SystemLanguage.Russian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.SerboCroatian}"), this.GetDescription(SystemLanguage.SerboCroatian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Slovak}"), this.GetDescription(SystemLanguage.Slovak), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Slovenian}"), this.GetDescription(SystemLanguage.Slovenian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Spanish}"), this.GetDescription(SystemLanguage.Spanish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Swedish}"), this.GetDescription(SystemLanguage.Swedish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Thai}"), this.GetDescription(SystemLanguage.Thai), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Turkish}"), this.GetDescription(SystemLanguage.Turkish), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Ukrainian}"), this.GetDescription(SystemLanguage.Ukrainian), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Vietnamese}"), this.GetDescription(SystemLanguage.Vietnamese), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.ChineseSimplified}"), this.GetDescription(SystemLanguage.ChineseSimplified), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.ChineseTraditional}"), this.GetDescription(SystemLanguage.ChineseTraditional), true);
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Hindi}"), this.GetDescription(SystemLanguage.Hindi), true);
        //
        // should be different to other langs logic
        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID($"{ModSettings.Flavor}{SystemLanguage.Unknown}"), LangConstants.OtherLanguagesSelect, true);

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
