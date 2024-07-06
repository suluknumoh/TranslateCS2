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
    public void SubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAlso) {
        // no need to realize
    }
    public void UnSubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAlso) {
        // no need to realize
    }
}
