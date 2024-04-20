using System.Collections.Generic;

using Colossal;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private readonly ModSettings m_Setting;
    public ModSettingsLocale(ModSettings setting) {
        this.m_Setting = setting;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return new Dictionary<string, string>
        {
                { this.m_Setting.GetSettingsLocaleID(), Mod.Name },
                // TODO: what is it for?
                { this.m_Setting.GetOptionTabLocaleID(ModSettings.kSection), ModSettings.kSection },

                { this.m_Setting.GetOptionGroupLocaleID(ModSettings.kButtonGroup), ModSettings.kButtonGroup },

                { this.m_Setting.GetOptionLabelLocaleID(nameof(ModSettings.ButtonWithConfirmation)), "reload language files" },
                { this.m_Setting.GetOptionDescLocaleID(nameof(ModSettings.ButtonWithConfirmation)), $"Reloads the language file(s)." },
                { this.m_Setting.GetOptionWarningLocaleID(nameof(ModSettings.ButtonWithConfirmation)), "Do you really want to reload the language file(s)?" }

            };
    }

    public void Unload() {

    }
}