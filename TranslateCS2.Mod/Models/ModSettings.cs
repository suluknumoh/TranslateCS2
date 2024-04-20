using Colossal.IO.AssetDatabase;
using Colossal.Json;

using Game.Modding;
using Game.Settings;

using TranslateCS2.Mod.Services;

namespace TranslateCS2.Mod.Models;
[FileLocation(Mod.Name)]
[SettingsUIGroupOrder(ButtonGroup)]
[SettingsUIShowGroupName(ButtonGroup)]
internal class ModSettings : ModSetting {
    public const string Section = "Main";

    public const string ButtonGroup = "Reload";

    private readonly TranslationFileService _fileHelper;


    [SettingsUIHidden]
    [Include]
    public string? Locale { get; set; }

    public ModSettings(IMod mod, TranslationFileService fileHelper) : base(mod) {
        this._fileHelper = fileHelper;
    }

    [SettingsUIButton]
    [SettingsUIConfirmation]
    [SettingsUISection(Section, ButtonGroup)]
    public bool ButtonWithConfirmation {
        set => this._fileHelper.Reload();
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
