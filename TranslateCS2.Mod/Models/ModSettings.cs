using System;
using System.IO;
using System.Text;

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;

using Game.Modding;
using Game.Settings;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Models;
/// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
[FileLocation($"{ModConstants.ModsSettings}/{ModConstants.Name}/{ModConstants.Name}")]
[SettingsUIGroupOrder(FlavorGroup, ReloadGroup, GenerateGroup)]
[SettingsUIShowGroupName(FlavorGroup, ReloadGroup, GenerateGroup)]
internal partial class ModSettings : ModSetting {
    [Exclude]
    private readonly ModRuntimeContainerHandler runtimeContainerHandler;
    [Exclude]
    private readonly IModRuntimeContainer runtimeContainer;
    [Exclude]
    private readonly MyLanguages languages;

    [Exclude]
    public ModSettingsLocale? SettingsLocale { get; set; }

    public const string Section = "Main";
    public const string FlavorGroup = nameof(FlavorGroup);
    public const string ReloadGroup = nameof(ReloadGroup);
    public const string GenerateGroup = nameof(GenerateGroup);


    [Include]
    [SettingsUIHidden]
    public string? Locale { get; set; }
    [Include]
    [SettingsUIHidden]
    public string? PreviousLocale { get; set; }

    public ModSettings(ModRuntimeContainerHandler runtimeContainerHandler, IMod mod) : base(mod) {
        this.runtimeContainerHandler = runtimeContainerHandler;
        this.runtimeContainer = runtimeContainerHandler.RuntimeContainer;
        this.languages = this.runtimeContainer.Languages;
        this.OnFlavorChanged += this.languages.FlavorChanged;
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
            this.runtimeContainer.IntSetting.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            this.runtimeContainer.IntSetting.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            this.runtimeContainer.LocManager.SetActiveLocale(this.runtimeContainer.IntSetting.locale);
            this.languages.ReLoad();
            this.runtimeContainer.IntSetting.currentLocale = this.Locale;
            this.runtimeContainer.IntSetting.locale = this.Locale;
            this.runtimeContainer.LocManager.SetActiveLocale(this.Locale);
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
    private bool IsGenerateLocalizationJsonHiddenDisabled() {
        return this.SettingsLocale == null;
    }
    private void GenerateLocJson() {
        if (this.IsGenerateLocalizationJsonHiddenDisabled()) {
            return;
        }
        try {
            try {
                string json = JsonConvert.SerializeObject(this.SettingsLocale.Dictionary, Formatting.Indented);
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





    public override void SetDefaults() {
        //
    }

    public void HandleLocaleOnLoad() {
        try {
            this.PreviousLocale = this.runtimeContainer.IntSetting.locale;
            if (this.runtimeContainer.LocManager.SupportsLocale(this.Locale)) {
                this.runtimeContainer.LocManager.SetActiveLocale(this.Locale);
                this.runtimeContainer.IntSetting.currentLocale = this.Locale;
                this.runtimeContainer.IntSetting.locale = this.Locale;
                this.OnLocaleChanged();
            }
            this.runtimeContainer.IntSetting.onSettingsApplied += this.ApplyAndSaveAlso;
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(HandleLocaleOnLoad), ex]);
        }
    }

    private void ApplyAndSaveAlso(Setting setting) {
        try {
            if (setting is InterfaceSettings interfaceSettings) {
                this.Locale = interfaceSettings.locale;
                this.OnLocaleChanged();
                this.ApplyAndSave();
            }
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
        string localeId = this.Getter(language.SystemLanguage);
        this.Setter(language.SystemLanguage, localeId);
    }

    public void HandleLocaleOnUnLoad() {
        try {
            // dont replicate os lang into this mods settings
            this.runtimeContainer.IntSetting.onSettingsApplied -= this.ApplyAndSaveAlso;
            // reset to os-language, if the mod is not used next time the game starts
            if (this.Locale != null && this.runtimeContainer.Locales.IsBuiltIn(this.Locale)) {
                this.runtimeContainer.IntSetting.currentLocale = this.Locale;
                this.runtimeContainer.IntSetting.locale = this.Locale;
            } else {
                this.runtimeContainer.IntSetting.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
                this.runtimeContainer.IntSetting.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            }
            this.runtimeContainer.IntSetting.ApplyAndSave();
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(HandleLocaleOnUnLoad), ex]);
        }
    }
}
