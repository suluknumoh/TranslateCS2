using Game.Settings;

using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items;
/// <summary>
///     wrapper for <see cref="InterfaceSettings"/>
/// </summary>
internal class IntSettings : IIntSettings {
    private readonly InterfaceSettings interfaceSettings;

    public IntSettings(InterfaceSettings interfaceSettings) {
        this.interfaceSettings = interfaceSettings;
    }

    public string CurrentLocale {
        get => this.interfaceSettings.currentLocale;
        set => this.interfaceSettings.currentLocale = value;
    }
    public string Locale {
        get => this.interfaceSettings.locale;
        set => this.interfaceSettings.locale = value;
    }

    public void ApplyAndSave() {
        this.interfaceSettings.ApplyAndSave();
    }

    public void SubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAndSaveAlso) {
        this.interfaceSettings.onSettingsApplied += applyAndSaveAlso;
    }

    public void UnSubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAndSaveAlso) {
        this.interfaceSettings.onSettingsApplied -= applyAndSaveAlso;
    }
}
