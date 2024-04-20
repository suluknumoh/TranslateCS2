using Colossal.IO.AssetDatabase;
using Colossal.Json;

using Game.Modding;
using Game.Settings;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Services;

namespace TranslateCS2.Mod.Models;
[FileLocation(Mod.Name)]
[SettingsUIGroupOrder(kButtonGroup)]
[SettingsUIShowGroupName(kButtonGroup)]
internal class ModSettings : ModSetting {
    public const string kSection = "Main";

    public const string kButtonGroup = "Reload";

    private readonly TranslationFileService _fileHelper;


    [SettingsUIHidden]
    [Include]
    public string? Locale { get; set; }

    public ModSettings(IMod mod, TranslationFileService fileHelper) : base(mod) {
        this._fileHelper = fileHelper;
    }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(kSection, kButtonGroup)]
    public bool ButtonWithConfirmation {
        set {
            //
            Mod.Logger.Info("ButtonWithConfirmation clicked");
            //
            this._fileHelper.Reload();
        }
    }

    public override void SetDefaults() {
        //
    }

    public void ApplyAndSaveAlso(Setting setting) {
        Mod.Logger.LogInfo(this.GetType(), "a");
        if (setting is InterfaceSettings interfaceSettings) {
            Mod.Logger.LogInfo(this.GetType(), "b");
            this.Locale = interfaceSettings.locale;
            Mod.Logger.LogInfo(this.GetType(), this.Locale);
            this.ApplyAndSave();
        }
    }
}
