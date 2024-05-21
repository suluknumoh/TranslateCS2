using Game.Settings;

using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.ZZZModTestLib.Containers.Items;
/// <summary>
///     imitates <see cref="Game.Settings.InterfaceSettings"/>' behaviour for testing purposes
/// </summary>
public class TestIntSettings : IIntSettings {
    public string CurrentLocale {
        get => this.Locale;
        set => this.Locale = value;
    }
    public string Locale { get; set; }
    public TestIntSettings() {
        this.Locale = "en-US";
    }
    public void ApplyAndSave() {
        // no need to realize
    }
    public void SubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAndSaveAlso) {
        // no need to realize
    }
    public void UnSubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAndSaveAlso) {
        // no need to realize
    }
}
