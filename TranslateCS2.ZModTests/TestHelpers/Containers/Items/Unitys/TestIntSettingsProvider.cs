using Game.Settings;

using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
/// <summary>
///     imitates <see cref="InterfaceSettings"/>' behaviour for testing purposes
/// </summary>
internal class TestIntSettingsProvider : IIntSettingsProvider {
    public string CurrentLocale {
        get => this.Locale;
        set => this.Locale = value;
    }
    public string Locale { get; set; }
    public TestIntSettingsProvider() {
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
