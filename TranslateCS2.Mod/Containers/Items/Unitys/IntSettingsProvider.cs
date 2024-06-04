using Game.Settings;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
/// <summary>
///     wrapper for <see cref="InterfaceSettings"/>
/// </summary>
[MyExcludeFromCoverage]
internal class IntSettingsProvider : IIntSettingsProvider {
    private readonly InterfaceSettings interfaceSettings;

    public IntSettingsProvider(InterfaceSettings interfaceSettings) {
        this.interfaceSettings = interfaceSettings;
    }

    public string CurrentLocale {
        get => this.interfaceSettings.currentLocale;
        set => this.interfaceSettings.currentLocale = value;
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
