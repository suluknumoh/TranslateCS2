using System;
using System.Collections.Generic;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Properties.I18N;

namespace TranslateCS2.Mod.Containers.Items;
internal class ModSettingsLocaleSource : IDictionarySource {
    private readonly Dictionary<string, string> _AllEntries = [];
    public IReadOnlyDictionary<string, string> AllEntries => this._AllEntries;
    private readonly Dictionary<string, string> _ExportableEntries = [];
    public IReadOnlyDictionary<string, string> ExportableEntries => this._ExportableEntries;
    public string LocaleId { get; }
    public bool IsFallBack { get; }
    public ModSettingsLocaleSource(ModSettings modSettings,
                                   string localeId,
                                   bool isFallBack) {
        this.LocaleId = localeId;
        this.IsFallBack = isFallBack;
        this.Init(modSettings);
    }

    private void Init(ModSettings modSettings) {
        string path = $"{typeof(I18NMod).Namespace}{StringConstants.Dot}{this.LocaleId}{ModConstants.JsonExtension}";
        IDictionary<string, string>? dictionary = JsonHelper.DeSerializeFromAssembly<Dictionary<string, string>>(path);
        if (dictionary is null) {
            return;
        }
        dictionary = DictionaryHelper.GetNonEmpty(dictionary);
        if (dictionary.Count == 0) {
            return;
        }
        if (this.IsFallBack) {
            this.AddToDictionary(modSettings.GetSettingsLocaleID(),
                                 ModConstants.NameSimple,
                                 false);
            // INFO: what is it for?
            this.AddToDictionary(modSettings.GetOptionTabLocaleID(ModSettings.Section),
                                 ModSettings.Section,
                                 false);
        }
        this.AddSettingsGroup(modSettings, dictionary);
        this.AddReloadGroup(modSettings, dictionary);
        this.AddGenerateGroup(modSettings, dictionary);
        this.AddFlavorGroup(modSettings, dictionary);
        this.AddExportGoup(modSettings, dictionary);
    }

    private void AddSettingsGroup(ModSettings modSettings, IDictionary<string, string> dictionary) {
        this.AddToDictionary(modSettings.GetOptionGroupLocaleID(ModSettings.SettingsGroup),
                             dictionary[I18NMod.GroupSettingsTitle]);
        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.LoadFromOtherMods)),
                             dictionary[I18NMod.GroupSettingsToggleLoadFromOtherModsLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.LoadFromOtherMods)),
                             dictionary[I18NMod.GroupSettingsToggleLoadFromOtherModsDescription]);
    }

    private void AddReloadGroup(ModSettings modSettings, IDictionary<string, string> dictionary) {
        this.AddToDictionary(modSettings.GetOptionGroupLocaleID(ModSettings.ReloadGroup),
                             dictionary[I18NMod.GroupReloadTitle]);
        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)),
                             dictionary[I18NMod.GroupReloadButtonReloadLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)),
                             dictionary[I18NMod.GroupReloadButtonReloadDescription]);
        this.AddToDictionary(modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ReloadLanguages)),
                             dictionary[I18NMod.GroupReloadButtonReloadConfirmation]);
    }

    private void AddGenerateGroup(ModSettings modSettings, IDictionary<string, string> dictionary) {
        this.AddToDictionary(modSettings.GetOptionGroupLocaleID(ModSettings.GenerateGroup),
                             dictionary[I18NMod.GroupGenerateTitle]);
        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.LogMarkdownAndCultureInfoNames)),
                             dictionary[I18NMod.GroupGenerateButtonLogMarkdownAndCultureInfoNamesLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.LogMarkdownAndCultureInfoNames)),
                             dictionary[I18NMod.GroupGenerateButtonLogMarkdownAndCultureInfoNamesDescription]);
        //
        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateDirectory)),
                             dictionary[I18NMod.GroupGenerateGenerateDirectoryLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateDirectory)),
                             this.Format(dictionary[I18NMod.GroupGenerateGenerateDirectoryDescription],
                                               ModConstants.ModExportKeyValueJsonName));
        //
        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                             dictionary[I18NMod.GroupGenerateButtonGenerateLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                             this.Format(dictionary[I18NMod.GroupGenerateButtonGenerateDescription],
                                               ModConstants.ModExportKeyValueJsonName));
        this.AddToDictionary(modSettings.GetOptionWarningLocaleID(nameof(ModSettings.GenerateLocalizationJson)),
                             dictionary[I18NMod.GroupGenerateButtonGenerateConfirmation]);
    }

    private void AddFlavorGroup(ModSettings modSettings, IDictionary<string, string> dictionary) {
        this.AddToDictionary(modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup),
                             dictionary[I18NMod.GroupFlavorTitle]);

        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.CurrentLanguage)),
                             dictionary[I18NMod.GroupFlavorCurrentLanguageLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.CurrentLanguage)),
                             dictionary[I18NMod.GroupFlavorCurrentLanguageDescription]);

        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDropDown)),
                             dictionary[I18NMod.GroupFlavorFlavorLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDropDown)),
                             dictionary[I18NMod.GroupFlavorFlavorDescription]);
    }

    private void AddExportGoup(ModSettings modSettings, IDictionary<string, string> dictionary) {
        this.AddToDictionary(modSettings.GetOptionGroupLocaleID(ModSettings.ExportGroup),
                             dictionary[I18NMod.GroupExportTitle]);

        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ExportDropDown)),
                             dictionary[I18NMod.GroupExportExportDropDownLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.ExportDropDown)),
                             dictionary[I18NMod.GroupExportExportDropDownDescription]);

        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ExportDirectory)),
                             dictionary[I18NMod.GroupExportExportDirectoryLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.ExportDirectory)),
                             dictionary[I18NMod.GroupExportExportDirectoryDescription]);

        this.AddToDictionary(modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ExportButton)),
                             dictionary[I18NMod.GroupExportExportButtonLabel]);
        this.AddToDictionary(modSettings.GetOptionDescLocaleID(nameof(ModSettings.ExportButton)),
                             dictionary[I18NMod.GroupExportExportButtonDescription]);
        this.AddToDictionary(modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ExportButton)),
                             dictionary[I18NMod.GroupExportExportButtonConfirmation]);
    }

    private string? Format(string? format, string? content) {
        if (StringHelper.IsNullOrWhiteSpaceOrEmpty(format)
            || StringHelper.IsNullOrWhiteSpaceOrEmpty(content)) {
            return null;
        }
        return String.Format(format, content);
    }

    private void AddToDictionary(string key,
                                 string? value) {
        this.AddToDictionary(key,
                             value,
                             this.IsFallBack);
    }

    private void AddToDictionary(string key,
                                 string? value,
                                 bool isExportable) {
        // double null check to avoid warning...
        if (value is null
            || StringHelper.IsNullOrWhiteSpaceOrEmpty(value)) {
            return;
        }
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
