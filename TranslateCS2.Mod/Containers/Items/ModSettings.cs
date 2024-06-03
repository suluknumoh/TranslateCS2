using System;
using System.IO;
using System.Text;

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;

using Game.Modding;
using Game.Settings;
using Game.UI.Menu;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Mod.Containers.Items;
/// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
[FileLocation($"{ModConstants.ModsSettings}/{ModConstants.Name}/{ModConstants.Name}")]
[SettingsUIGroupOrder(FlavorGroup, ReloadGroup, GenerateGroup)]
[SettingsUIShowGroupName(FlavorGroup, ReloadGroup, GenerateGroup)]
internal partial class ModSettings : ModSetting {




    public const string Section = "Main";
    public const string FlavorGroup = nameof(FlavorGroup);
    public const string ReloadGroup = nameof(ReloadGroup);
    public const string GenerateGroup = nameof(GenerateGroup);




    [Exclude]
    private readonly IModRuntimeContainer runtimeContainer;
    [Exclude]
    private readonly MyLanguages languages;

    [Exclude]
    public ModSettingsLocale? SettingsLocale { get; set; }


    [Include]
    [SettingsUIHidden]
    public string Locale { get; set; }
    [Include]
    [SettingsUIHidden]
    public string PreviousLocale { get; set; }

    public ModSettings(IModRuntimeContainer runtimeContainer) : base(runtimeContainer.Mod) {
        this.runtimeContainer = runtimeContainer;
        this.languages = this.runtimeContainer.Languages;
        this.Locale = this.runtimeContainer.IntSettings.CurrentLocale;
        this.PreviousLocale = this.Locale;
        this.SubscribeOnFlavorChanged(this.runtimeContainer.LocManager.FlavorChanged);
    }

    [Exclude]
    [SettingsUIButton]
    [SettingsUIDeveloper]
    [SettingsUIConfirmation]
    [SettingsUISection(Section, ReloadGroup)]
    public bool ReloadLanguages {
        set => this.ReloadLangs();
    }

    private void ReloadLangs() {
        try {
            this.languages.ReLoad();
            if (this.languages.HasErroneous) {
                this.runtimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(this.languages.Erroneous, true);
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(ReloadLangs), ex]);
        }
    }

    [Exclude]
    [SettingsUIButton]
    [SettingsUIDeveloper]
    [SettingsUISection(Section, GenerateGroup)]
    public bool LogMarkdownAndCultureInfoNames {
        set => this.languages.LogMarkdownAndCultureInfoNames();
    }

    [Exclude]
    [SettingsUIButton]
    [SettingsUIDeveloper]
    [SettingsUISection(Section, GenerateGroup)]
    [SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsGenerateLocalizationJsonHiddenDisabled))]
    [SettingsUIHideByCondition(typeof(ModSettings), nameof(IsGenerateLocalizationJsonHiddenDisabled))]
    public bool GenerateLocalizationJson {
        set => this.GenerateLocJson();
    }
    public bool IsGenerateLocalizationJsonHiddenDisabled() {
        return this.SettingsLocale is null;
    }
    private void GenerateLocJson() {
        if (this.SettingsLocale is null) {
            return;
        }
        try {
            try {
                string json = JsonConvert.SerializeObject(this.SettingsLocale.ExportableEntries, Formatting.Indented);
                string path = Path.Combine(this.runtimeContainer.Paths.ModsDataPathSpecific, ModConstants.ModExportKeyValueJsonName);
                File.WriteAllText(path, json, Encoding.UTF8);
            } catch (Exception ex) {
                this.runtimeContainer.ErrorMessages.DisplayErrorMessageFailedToGenerateJson();
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(this.GenerateLocalizationJson), ex]);
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(this.GenerateLocalizationJson), ex]);
        }
    }




    [MyExcludeFromCoverage]
    public override AutomaticSettings.SettingPageData GetPageData(string id, bool addPrefix) {
        AutomaticSettings.SettingPageData pageData = base.GetPageData(id, addPrefix);
        this.AddFlavorsToPageData(pageData);
        return pageData;
    }
    [MyExcludeFromCoverage]
    public override void SetDefaults() {
        //
    }
    public void HandleLocaleOnLoad() {
        try {
            this.PreviousLocale = this.runtimeContainer.IntSettings.CurrentLocale;
            if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Locale)) {
                // if this locale is null after it is loaded
                this.Locale = this.PreviousLocale;
            }
            if (this.runtimeContainer.LocManager.SupportsLocale(this.Locale)) {
                this.runtimeContainer.LocManager.SetActiveLocale(this.Locale);
                this.runtimeContainer.IntSettings.CurrentLocale = this.Locale;
                this.OnLocaleChanged();
            }
            this.runtimeContainer.IntSettings.SubscribeOnSettingsApplied(this.ApplyAndSaveAlso);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(HandleLocaleOnLoad), ex]);
        }
    }
    public void HandleLocaleOnUnLoad() {
        try {
            // dont replicate os lang into this mods settings
            this.runtimeContainer.IntSettings.UnSubscribeOnSettingsApplied(this.ApplyAndSaveAlso);
            // reset to os-language, if the mod is not used next time the game starts
            if (this.Locale != null
                && this.runtimeContainer.Locales.IsBuiltIn(this.Locale)) {
                this.runtimeContainer.IntSettings.CurrentLocale = this.Locale;
            } else {
                this.runtimeContainer.IntSettings.CurrentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            }
            this.runtimeContainer.IntSettings.ApplyAndSave();
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(HandleLocaleOnUnLoad), ex]);
        }
    }
    private void ApplyAndSaveAlso(Setting setting) {
        if (setting is not InterfaceSettings interfaceSettings) {
            return;
        }
        try {
            this.Locale = interfaceSettings.locale;
            this.OnLocaleChanged();
            this.ApplyAndSave();
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(ApplyAndSaveAlso), ex]);
        }
    }
    private void OnLocaleChanged() {
        if (this.Locale is null) {
            return;
        }
        MyLanguage? language = this.languages.GetLanguage(this.Locale);
        if (language is null) {
            return;
        }
        string localeId = this.GetSettedFlavor(language.SystemLanguage);
        this.SetFlavor(language.SystemLanguage, localeId);
    }

}
