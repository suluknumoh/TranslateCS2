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

    public void SubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAlso) {
        this.interfaceSettings.onSettingsApplied += applyAlso;
    }

    public void UnSubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAlso) {
        this.interfaceSettings.onSettingsApplied -= applyAlso;
    }
}
