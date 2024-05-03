using System;
using System.IO;
using System.Text;

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;

using Game.Modding;
using Game.SceneFlow;
using Game.Settings;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Models;
/// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
[FileLocation($"{ModConstants.ModsSettings}/{ModConstants.Name}/{ModConstants.Name}")]
[SettingsUIGroupOrder(FlavorGroup, ReloadGroup, GenerateGroup)]
[SettingsUIShowGroupName(FlavorGroup, ReloadGroup, GenerateGroup)]
internal partial class ModSettings : ModSetting {
    private static GameManager GameManager { get; } = GameManager.instance;
    private static InterfaceSettings InterfaceSettings { get; } = ModSettings.GameManager.settings.userInterface;
    private static LocalizationManager LocalizationManager { get; } = ModSettings.GameManager.localizationManager;
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

    public ModSettings(IMod mod) : base(mod) { }

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
            MyLanguages languages = MyLanguages.Instance;
            ModSettings.InterfaceSettings.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.InterfaceSettings.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.LocalizationManager.SetActiveLocale(ModSettings.InterfaceSettings.locale);
            languages.ReLoad();
            ModSettings.InterfaceSettings.currentLocale = this.Locale;
            ModSettings.InterfaceSettings.locale = this.Locale;
            ModSettings.LocalizationManager.SetActiveLocale(this.Locale);
            if (languages.HasErroneous) {
                ErrorMessageHelper.DisplayErrorMessageForErroneous(languages.Erroneous, true);
            }
        } catch (Exception ex) {
            Mod.Logger.LogCritical(this.GetType(),
                                   LoggingConstants.FailedTo,
                                   [nameof(ReloadLangs), ex]);
        }
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
        string json = JsonConvert.SerializeObject(this.SettingsLocale.Dictionary, Formatting.Indented);
        string path = Path.Combine(FileSystemHelper.DataFolder, ModConstants.ModExportKeyValueJsonName);
        File.WriteAllText(path, json, Encoding.UTF8);
    }





    public override void SetDefaults() {
        //
    }

    public void HandleLocaleOnLoad() {
        try {
            this.PreviousLocale = ModSettings.InterfaceSettings.locale;
            if (ModSettings.LocalizationManager.SupportsLocale(this.Locale)) {
                ModSettings.LocalizationManager.SetActiveLocale(this.Locale);
                ModSettings.InterfaceSettings.currentLocale = this.Locale;
                ModSettings.InterfaceSettings.locale = this.Locale;
            }
            ModSettings.InterfaceSettings.onSettingsApplied += this.ApplyAndSaveAlso;
        } catch (Exception ex) {
            Mod.Logger.LogCritical(this.GetType(),
                                   LoggingConstants.FailedTo,
                                   [nameof(HandleLocaleOnLoad), ex]);
        }
    }

    private void ApplyAndSaveAlso(Setting setting) {
        try {
            if (setting is InterfaceSettings interfaceSettings) {
                this.Locale = interfaceSettings.locale;
                this.ApplyAndSave();
            }
        } catch (Exception ex) {
            Mod.Logger.LogCritical(this.GetType(),
                                   LoggingConstants.FailedTo,
                                   [nameof(ApplyAndSaveAlso), ex]);
        }
    }

    public void HandleLocaleOnUnLoad() {
        try {
            // dont replicate os lang into this mods settings
            ModSettings.InterfaceSettings.onSettingsApplied -= this.ApplyAndSaveAlso;
            // reset to os-language, if the mod is not used next time the game starts
            if (this.Locale != null && LocaleHelper.IsBuiltIn(this.Locale)) {
                ModSettings.InterfaceSettings.currentLocale = this.Locale;
                ModSettings.InterfaceSettings.locale = this.Locale;
            } else {
                ModSettings.InterfaceSettings.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
                ModSettings.InterfaceSettings.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            }
            ModSettings.InterfaceSettings.ApplyAndSave();
        } catch (Exception ex) {
            Mod.Logger.LogCritical(this.GetType(),
                                   LoggingConstants.FailedTo,
                                   [nameof(HandleLocaleOnUnLoad), ex]);
        }
    }
}
