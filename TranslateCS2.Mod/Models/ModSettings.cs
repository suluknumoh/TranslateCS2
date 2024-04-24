using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;

using Game.Modding;
using Game.SceneFlow;
using Game.Settings;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Models;
/// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
[FileLocation($"{ModConstants.ModsSettings}/{ModConstants.Name}/{ModConstants.Name}")]
[SettingsUIGroupOrder(BehaviourGroup, ClearGroup, ReloadGroup)]
[SettingsUIShowGroupName(BehaviourGroup, ClearGroup, ReloadGroup)]
internal class ModSettings : ModSetting {
    // TODO: refactor
    // TODO: enable/disable/hide items based on the detected language files
    // TODO: is it possible to make flavors selectable? the general language code is not used, so it should be possible to add for example 'nl' as general language and add the selected flavors
    // TODO: is it possible fill and refill a single drop down or is SettingsUIDropDown->itemsGetterMethod called only once? if it is, add drop downs for each possible UnityEngine.SystemLanguage and use SettingsUIHideByCondition
    private static GameManager GameManager { get; } = GameManager.instance;
    private static InterfaceSettings InterfaceSettings { get; } = ModSettings.GameManager.settings.userInterface;
    private static LocalizationManager LocalizationManager { get; } = ModSettings.GameManager.localizationManager;


    public const string Section = "Main";
    public const string BehaviourGroup = nameof(BehaviourGroup);
    public const string ClearGroup = nameof(ClearGroup);
    public const string ReloadGroup = nameof(ReloadGroup);


    [Include]
    [SettingsUIHidden]
    public string? Locale { get; set; }
    [Include]
    [SettingsUIHidden]
    public string? PreviousLocale { get; set; }

    public ModSettings(IMod mod) : base(mod) {
    }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(Section, ReloadGroup)]
    public bool ReloadLanguages {
        set {
            ModSettings.InterfaceSettings.currentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.InterfaceSettings.locale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
            ModSettings.LocalizationManager.SetActiveLocale(ModSettings.InterfaceSettings.locale);
            MyCountrys.Instance.Reload();
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
        set => MyCountrys.Instance.ClearOverwritten();
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

    public void HandleLocaleOnUnLoad() {
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
