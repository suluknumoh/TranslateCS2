using Game.Settings;

using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items;
internal class IntSettings {
    public IIntSettingsProvider Provider { get; }
    public IntSettings(IIntSettingsProvider provider) {
        this.Provider = provider;
    }
    public string CurrentLocale {
        get => this.Provider.CurrentLocale;
        set => this.Provider.CurrentLocale = value;
    }

    public void SubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAlso) {
        this.Provider.SubscribeOnSettingsApplied(applyAlso);
    }

    public void UnSubscribeOnSettingsApplied(OnSettingsAppliedHandler applyAlso) {
        this.Provider.UnSubscribeOnSettingsApplied(applyAlso);
    }
}
