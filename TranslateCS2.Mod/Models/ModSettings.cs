using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;

using Game.Modding;
using Game.SceneFlow;
using Game.Settings;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Services;

namespace TranslateCS2.Mod.Models;
[FileLocation(Mod.Name)]
[SettingsUIGroupOrder(BehaviourGroup, ClearGroup, ReloadGroup)]
[SettingsUIShowGroupName(BehaviourGroup, ClearGroup, ReloadGroup)]
internal class ModSettings : ModSetting {
    private static GameManager GameManager { get; } = GameManager.instance;
    private static InterfaceSettings InterfaceSettings { get; } = ModSettings.GameManager.settings.userInterface;
    private static LocalizationManager LocalizationManager { get; } = ModSettings.GameManager.localizationManager;


    public const string Section = "Main";
    public const string BehaviourGroup = "Behaviour";
    public const string ClearGroup = "Clear";
    public const string ReloadGroup = "Reload";

    private readonly TranslationFileService _translationFileService;


    [Include]
    [SettingsUIHidden]
    public string? Locale { get; set; }
    [Include]
    [SettingsUIHidden]
    public string? PreviousLocale { get; set; }

    public ModSettings(IMod mod, TranslationFileService fileHelper) : base(mod) {
        this._translationFileService = fileHelper;
        this._translationFileService.Settings = this;
    }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(Section, ReloadGroup)]
    public bool ReloadLanguages {
        set {
            ModSettings.InterfaceSettings.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.InterfaceSettings.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.LocalizationManager.SetActiveLocale(ModSettings.InterfaceSettings.locale);
            this._translationFileService.Reload();
            ModSettings.InterfaceSettings.currentLocale = this.Locale;
            ModSettings.InterfaceSettings.locale = this.Locale;
            ModSettings.LocalizationManager.SetActiveLocale(this.Locale);
        }
    }

    [Include]
    [SettingsUISection(Section, BehaviourGroup)]
    public bool IsOverwrite { get; set; }


    public override void SetDefaults() {
        //
    }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(Section, ClearGroup)]
    //[SettingsUIDisableByCondition(typeof(ModSettings), "IsClearOverwrittenAllowed")]
    public bool ClearOverwritten {
        set => this._translationFileService.ClearOverwritten();
    }

    public bool IsClearOverwrittenAllowed() {
        return !this.IsOverwrite;
    }

    public void HandleLocaleOnLoad() {
        this.PreviousLocale = ModSettings.InterfaceSettings.locale;
        if (ModSettings.LocalizationManager.SupportsLocale(this.Locale)) {
            ModSettings.LocalizationManager.SetActiveLocale(this.Locale);
            ModSettings.InterfaceSettings.currentLocale = this.Locale;
            ModSettings.InterfaceSettings.locale = this.Locale;
        }
        ModSettings.InterfaceSettings.onSettingsApplied += this.ApplyAndSaveAlso;
    }

    private void ApplyAndSaveAlso(Setting setting) {
        if (setting is InterfaceSettings interfaceSettings) {
            this.Locale = interfaceSettings.locale;
            this.ApplyAndSave();
        }
    }

    internal void HandleLocaleOnUnLoad() {
        // dont replicate os lang into this mods settings
        ModSettings.InterfaceSettings.onSettingsApplied -= this.ApplyAndSaveAlso;
        // reset to os-language, if the mod is not used next time the game starts
        if (this.Locale != null && LocaleHelper.BuiltIn.Contains(this.Locale)) {
            ModSettings.InterfaceSettings.currentLocale = this.Locale;
            ModSettings.InterfaceSettings.locale = this.Locale;
        } else {
            ModSettings.InterfaceSettings.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.InterfaceSettings.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
        }
        ModSettings.InterfaceSettings.ApplyAndSave();
    }
}
