using System;
using System.Collections.Generic;
using System.Linq;

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
        this.AddToDictionary(this.modSettings.GetSettingsLocaleID(),
                             ModConstants.NameSimple,
                             false);
        // INFO: what is it for?
        this.AddToDictionary(this.modSettings.GetOptionTabLocaleID(ModSettings.Section),
                             ModSettings.Section,
                             false);
        {
        // reload-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.ReloadGroup),
                                 I18NMod.GroupReloadTitle,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)),
                                 I18NMod.GroupReloadButtonReloadLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)),
                                 String.Format(I18NMod.GroupReloadButtonReloadDescription, this.runtimeContainer.Paths.ModsDataPathSpecific),
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ReloadLanguages)),
                                 I18NMod.GroupReloadButtonReloadWarning,
                                 true);
        }
        {
        // generate-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.GenerateGroup),
                                 I18NMod.GroupGenerateTitle,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.LogMarkdownAndCultureInfoNames)),
                                 I18NMod.GroupGenerateButtonLogMarkdownAndCultureInfoNamesLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.LogMarkdownAndCultureInfoNames)),
                                 I18NMod.GroupGenerateButtonLogMarkdownAndCultureInfoNamesDescription,
                                 true);
        //
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                                 I18NMod.GroupGenerateButtonGenerateLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                                 String.Format(I18NMod.GroupGenerateButtonGenerateDescription, ModConstants.ModExportKeyValueJsonName, this.runtimeContainer.Paths.ModsDataPathSpecific),
                                 true);
        }
        // flavor-group
        this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup), I18NMod.GroupFlavorTitle, true);


        // flavor-group-labels: test
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            string optionName = $"{ModSettings.Flavor}{systemLanguage}";
            if (systemLanguage is SystemLanguage.Unknown) {
                // should be different to other langs logic
                this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(optionName), LangConstants.OtherLanguages, true);
                this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(optionName), LangConstants.OtherLanguagesSelect, true);
                continue;
            } else if (systemLanguage is SystemLanguage.Chinese) {
                // chinese simplified and traditional are already present
                continue;
            }
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(optionName), this.GetLabel(systemLanguage), true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(optionName), this.GetDescription(systemLanguage), true);
        }
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
