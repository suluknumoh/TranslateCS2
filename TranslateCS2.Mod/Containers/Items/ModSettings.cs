using System;

using Colossal.IO.AssetDatabase;
using Colossal.Json;
using Colossal.Localization;

using Game.Modding;
using Game.Settings;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
/// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
[FileLocation($"{ModConstants.ModsSettings}/{ModConstants.Name}/{ModConstants.Name}")]
[SettingsUIGroupOrder(FlavorGroup, SettingsGroup, ReloadGroup, GenerateGroup, ExportGroup)]
[SettingsUIShowGroupName(FlavorGroup, SettingsGroup, ReloadGroup, GenerateGroup, ExportGroup)]
internal partial class ModSettings : ModSetting {




    public const string Section = "Main";




    [Exclude]
    private readonly IModRuntimeContainer runtimeContainer;
    [Exclude]
    private readonly MyLanguages languages;

    [Exclude]
    public ModSettingsLocale? SettingsLocale { get; set; }


    /// <summary>
    ///     has to be <see cref="MyLanguage.Id"/>
    /// </summary>
    [Include]
    [SettingsUIHidden]
    public string Locale { get; set; }

    /// <summary>
    ///     <see cref="InterfaceSettings.currentLocale"/> on startup
    ///     <br/>
    ///     before this mod is loaded
    /// </summary>
    [Include]
    [SettingsUIHidden]
    public string PreviousLocale { get; set; }

    public ModSettings(IModRuntimeContainer runtimeContainer) : base(runtimeContainer.Mod) {
        this.runtimeContainer = runtimeContainer;
        this.languages = this.runtimeContainer.Languages;
        this.Locale = this.runtimeContainer.IntSettings.CurrentLocale;
        this.PreviousLocale = this.Locale;
        this.SubscribeOnFlavorChanged(this.runtimeContainer.LocManager.FlavorChanged);
        this.SetDefaults();
    }





    [MyExcludeFromCoverage]
    public override void SetDefaults() {
        this.ExportDropDown = StringConstants.All;
        this.ExportDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        this.LoadFromOtherMods = true;
    }
    public void HandleLocaleOnLoad() {
        try {
            this.PreviousLocale = this.runtimeContainer.IntSettings.CurrentLocale;
            if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Locale)) {
                // if this locale is null after it is loaded
                this.Locale = this.PreviousLocale;
            }
            if (this.runtimeContainer.LocManager.SupportsLocale(this.Locale)) {
                this.runtimeContainer.IntSettings.CurrentLocale = this.Locale;
                this.runtimeContainer.LocManager.SetActiveLocale(this.Locale);
                this.OnLocaleChanged();
            }
            this.runtimeContainer.IntSettings.SubscribeOnSettingsApplied(this.Apply);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(HandleLocaleOnLoad), ex]);
        }
    }
    public void HandleLocaleOnUnLoad() {
        try {
            // dont replicate os lang into this mods settings
            this.runtimeContainer.IntSettings.UnSubscribeOnSettingsApplied(this.Apply);
            // reset to os-language, if the mod is not used next time the game starts
            string? intSettingsCurrentLocale = this.runtimeContainer.IntSettings.CurrentLocale;
            if (!this.runtimeContainer.Locales.IsBuiltIn(this.Locale)
                || !this.runtimeContainer.Locales.IsBuiltIn(intSettingsCurrentLocale)) {
                this.runtimeContainer.IntSettings.CurrentLocale = this.PreviousLocale ?? LocalizationManager.kOsLanguage;
                this.runtimeContainer.SettingsSaver?.SaveSettingsNow();
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(HandleLocaleOnUnLoad), ex]);
        }
    }
    /// <summary>
    ///     can not be tested
    ///     <br/>
    ///     <seealso cref="Game.Settings.OnSettingsAppliedHandler"/>
    ///     <br/>
    ///     <paramref name="setting"/> requires a <see cref="Setting"/>-<see langword="object"/>
    ///     <br/>
    ///     <see cref="IModRuntimeContainer"/> works with an <see langword="interface"/>/wrapper: <see cref="IModRuntimeContainer.IntSettings"/>
    ///     <br/>
    ///     so its excluded from coverage
    /// </summary>
    /// <param name="setting"></param>
    [MyExcludeFromCoverage]
    private void Apply(Setting setting) {
        if (setting is not InterfaceSettings interfaceSettings) {
            return;
        }
        try {
            this.Locale = interfaceSettings.locale;
            this.OnLocaleChanged();
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogCritical(this.GetType(),
                                                     LoggingConstants.FailedTo,
                                                     [nameof(Apply), ex]);
        }
    }
    private void OnLocaleChanged() {
        if (this.Locale is null) {
            return;
        }
        if (this.runtimeContainer.Locales.IsBuiltIn(this.Locale)) {
            this.PreviousLocale = this.Locale;
        }
        SystemLanguage systemLanguage = this.runtimeContainer.LocManager.LocaleIdToSystemLanguage(this.Locale);
        string flavorId = this.GetSettedFlavor(systemLanguage);
        this.FlavorDropDown = flavorId;
    }
}
