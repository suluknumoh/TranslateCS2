using System;
using System.Collections.Generic;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Properties.I18N;

namespace TranslateCS2.Mod.Containers.Items;
internal class ModSettingsLocale : IDictionarySource {
    private readonly ModSettings modSettings;
    private readonly Dictionary<string, string> _AllEntries = [];
    public IReadOnlyDictionary<string, string> AllEntries => this._AllEntries;
    private readonly Dictionary<string, string> _ExportableEntries = [];
    public IReadOnlyDictionary<string, string> ExportableEntries => this._ExportableEntries;
    public ModSettingsLocale(ModSettings modSettings) {
        this.modSettings = modSettings;
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
            // settings-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.SettingsGroup),
                                 I18NMod.GroupSettingsTitle,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.LoadFromOtherMods)),
                                 I18NMod.GroupSettingsToggleLoadFromOtherModsLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.LoadFromOtherMods)),
                                 I18NMod.GroupSettingsToggleLoadFromOtherModsDescription,
                                 true);
        }
        {
            // reload-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.ReloadGroup),
                                 I18NMod.GroupReloadTitle,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)),
                                 I18NMod.GroupReloadButtonReloadLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)),
                                 I18NMod.GroupReloadButtonReloadDescription,
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
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateDirectory)),
                                 I18NMod.GroupGenerateGenerateDirectoryLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateDirectory)),
                                 String.Format(I18NMod.GroupGenerateGenerateDirectoryDescription, ModConstants.ModExportKeyValueJsonName),
                                 true);
            //
            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                                 I18NMod.GroupGenerateButtonGenerateLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                                 String.Format(I18NMod.GroupGenerateButtonGenerateDescription, ModConstants.ModExportKeyValueJsonName),
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionWarningLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                                 I18NMod.GroupGenerateButtonGenerateConfirmation,
                                 true);
        }
        {
            // flavor-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup),
                                 I18NMod.GroupFlavorTitle,
                                 true);

            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.CurrentLanguage)),
                                 I18NMod.CurrentLanguageLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.CurrentLanguage)),
                                 I18NMod.CurrentLanguageDescription,
                                 true);

            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDropDown)),
                                 I18NMod.FlavorLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDropDown)),
                                 I18NMod.FlavorDescription,
                                 true);
        }
        {
            // export-group
            this.AddToDictionary(this.modSettings.GetOptionGroupLocaleID(ModSettings.ExportGroup),
                                 I18NMod.GroupExportTitle,
                                 true);

            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ExportDropDown)),
                                 I18NMod.GroupExportExportDropDownLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ExportDropDown)),
                                 I18NMod.GroupExportExportDropDownDescription,
                                 true);

            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ExportDirectory)),
                                 I18NMod.GroupExportExportDirectoryLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ExportDirectory)),
                                 I18NMod.GroupExportExportDirectoryDescription,
                                 true);

            this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ExportButton)),
                                 I18NMod.GroupExportExportButtonLabel,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ExportButton)),
                                 I18NMod.GroupExportExportButtonDescription,
                                 true);
            this.AddToDictionary(this.modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ExportButton)),
                                 I18NMod.GroupExportExportButtonWarning,
                                 true);
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

    public void Unload() { }
}
