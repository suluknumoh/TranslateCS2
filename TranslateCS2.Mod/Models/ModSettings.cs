using Colossal.IO.AssetDatabase;
using Colossal.Json;

using Game.Modding;
using Game.Settings;

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
        if (setting is InterfaceSettings interfaceSettings) {
            this.Locale = interfaceSettings.locale;
            this.ApplyAndSave();
        }
    }
}
