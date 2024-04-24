using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Properties.I18N;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private readonly ModSettings m_Setting;
    public ModSettingsLocale(ModSettings setting) {
        this.m_Setting = setting;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return new Dictionary<string, string>
        {
                { this.m_Setting.GetSettingsLocaleID(), ModConstants.NameSimple },
                // TODO: what is it for?
                { this.m_Setting.GetOptionTabLocaleID(ModSettings.Section), ModSettings.Section },

                // behaviour-group
                { this.m_Setting.GetOptionGroupLocaleID(ModSettings.BehaviourGroup), I18NMod.GroupBehaviourTitle },
                { this.m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.IsOverwrite)), I18NMod.GroupBehaviourOverwriteLabel },
                { this.m_Setting.GetOptionDescLocaleID(nameof(ModSettings.IsOverwrite)), I18NMod.GroupBehaviourOverwriteDescription },
                // reload-group
                { this.m_Setting.GetOptionGroupLocaleID(ModSettings.ReloadGroup), I18NMod.GroupReloadTitle },
                { this.m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadLabel },
                { this.m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadDescription },
                { this.m_Setting.GetOptionWarningLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadWarning },
                // clear-group
                { this.m_Setting.GetOptionGroupLocaleID(ModSettings.ClearGroup), I18NMod.GroupClearTitle },
                { this.m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ClearOverwritten)), I18NMod.GroupClearButtonOverwriteLabel },
                { this.m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ClearOverwritten)), I18NMod.GroupClearButtonOverwriteDescription },
                { this.m_Setting.GetOptionWarningLocaleID(nameof(ModSettings.ClearOverwritten)), I18NMod.GroupClearButtonOverwriteWarning },
            };
    }

    public void Unload() { }
}