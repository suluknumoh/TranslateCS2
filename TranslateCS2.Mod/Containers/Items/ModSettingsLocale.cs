using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Models;
using TranslateCS2.Mod.Properties.I18N;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
public class ModSettingsLocale : IDictionarySource {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly MyLanguages languages;
    private readonly ModSettings modSettings;
    private readonly Dictionary<string, string> _AllEntries = [];
    public IReadOnlyDictionary<string, string> AllEntries => this._AllEntries;
    private readonly Dictionary<string, string> _ExportableEntries = [];
    public IReadOnlyDictionary<string, string> ExportableEntries => this._ExportableEntries;
    public ModSettingsLocale(IModRuntimeContainer runtimeContainer) {
        this.modSettings = runtimeContainer.Settings;
        this.runtimeContainer = runtimeContainer;
        this.modSettings.SettingsLocale = this;
        this.languages = this.runtimeContainer.Languages;
    }

    public void Init() {
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
        {
            // flavor-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup),
                                 I18NMod.GroupFlavorTitle,
                                 true);


            IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
            foreach (SystemLanguage systemLanguage in systemLanguages) {
                string optionName = ModSettings.GetFlavorLangPropertyName(systemLanguage);
                switch (systemLanguage) {
                    case SystemLanguage.Chinese:
                        // chinese simplified and traditional are already present
                        break;
                    case SystemLanguage.Unknown:
                        // should be different to other langs logic
                        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(optionName),
                                             LangConstants.OtherLanguages,
                                             true);
                        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(optionName),
                                             LangConstants.OtherLanguagesSelect,
                                             true);
                        break;
                    default:
                        this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(optionName),
                                             this.GetLabel(systemLanguage),
                                             true);
                        this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(optionName),
                                             this.GetDescription(systemLanguage),
                                             true);
                        break;
                }
            }
        }
    }


    private void AddToDictionary(string key,
                                 string value,
                                 bool isExportable) {
        this._AllEntries.Add(key, value);
        if (isExportable) {
            this._ExportableEntries.Add(key, value);
        }
    }


    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return this._AllEntries;
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
        if (language is null) {
            return systemLanguage.ToString();
        }
        return language.NameEnglish;
    }

    public void Unload() { }
}
