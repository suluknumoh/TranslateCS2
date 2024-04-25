using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Properties.I18N;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private readonly ModSettings modSettings;
    public ModSettingsLocale(ModSettings setting) {
        this.modSettings = setting;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return new Dictionary<string, string>
        {
                { this.modSettings.GetSettingsLocaleID(), ModConstants.NameSimple },
                // INFO: what is it for?
                { this.modSettings.GetOptionTabLocaleID(ModSettings.Section), ModSettings.Section },

                // reload-group
                { this.modSettings.GetOptionGroupLocaleID(ModSettings.ReloadGroup), I18NMod.GroupReloadTitle },
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadLabel },
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadDescription },
                { this.modSettings.GetOptionWarningLocaleID(nameof(ModSettings.ReloadLanguages)), I18NMod.GroupReloadButtonReloadWarning },
                // flavor-group
                { this.modSettings.GetOptionGroupLocaleID(ModSettings.FlavorGroup), I18NMod.GroupFlavorTitle },

                // TODO: labels and descriptions

                // flavor-group-labels: test
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorAfrikaans)), nameof(ModSettings.FlavorAfrikaans)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorArabic)), nameof(ModSettings.FlavorArabic)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBasque)), nameof(ModSettings.FlavorBasque)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBelarusian)), nameof(ModSettings.FlavorBelarusian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBulgarian)), nameof(ModSettings.FlavorBulgarian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCatalan)), nameof(ModSettings.FlavorCatalan)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCzech)), nameof(ModSettings.FlavorCzech)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDanish)), nameof(ModSettings.FlavorDanish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDutch)), nameof(ModSettings.FlavorDutch)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEnglish)), nameof(ModSettings.FlavorEnglish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEstonian)), nameof(ModSettings.FlavorEstonian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFaroese)), nameof(ModSettings.FlavorFaroese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFinnish)), nameof(ModSettings.FlavorFinnish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFrench)), nameof(ModSettings.FlavorFrench)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGerman)), nameof(ModSettings.FlavorGerman)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGreek)), nameof(ModSettings.FlavorGreek)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHebrew)), nameof(ModSettings.FlavorHebrew)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIcelandic)), nameof(ModSettings.FlavorIcelandic)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIndonesian)), nameof(ModSettings.FlavorIndonesian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorItalian)), nameof(ModSettings.FlavorItalian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorJapanese)), nameof(ModSettings.FlavorJapanese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorKorean)), nameof(ModSettings.FlavorKorean)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLatvian)), nameof(ModSettings.FlavorLatvian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLithuanian)), nameof(ModSettings.FlavorLithuanian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorNorwegian)), nameof(ModSettings.FlavorNorwegian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPolish)), nameof(ModSettings.FlavorPolish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPortuguese)), nameof(ModSettings.FlavorPortuguese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRomanian)), nameof(ModSettings.FlavorRomanian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRussian)), nameof(ModSettings.FlavorRussian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSerboCroatian)), nameof(ModSettings.FlavorSerboCroatian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovak)), nameof(ModSettings.FlavorSlovak)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovenian)), nameof(ModSettings.FlavorSlovenian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSpanish)), nameof(ModSettings.FlavorSpanish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSwedish)), nameof(ModSettings.FlavorSwedish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorThai)), nameof(ModSettings.FlavorThai)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorTurkish)), nameof(ModSettings.FlavorTurkish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorUkrainian)), nameof(ModSettings.FlavorUkrainian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorVietnamese)), nameof(ModSettings.FlavorVietnamese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseSimplified)), nameof(ModSettings.FlavorChineseSimplified)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseTraditional)), nameof(ModSettings.FlavorChineseTraditional)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHindi)), nameof(ModSettings.FlavorHindi)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHungarian)), nameof(ModSettings.FlavorHungarian)},


                // flavor-group-descriptions: test
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorAfrikaans)), nameof(ModSettings.FlavorAfrikaans)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorArabic)), nameof(ModSettings.FlavorArabic)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBasque)), nameof(ModSettings.FlavorBasque)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBelarusian)), nameof(ModSettings.FlavorBelarusian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBulgarian)), nameof(ModSettings.FlavorBulgarian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCatalan)), nameof(ModSettings.FlavorCatalan)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCzech)), nameof(ModSettings.FlavorCzech)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDanish)), nameof(ModSettings.FlavorDanish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDutch)), nameof(ModSettings.FlavorDutch)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEnglish)), nameof(ModSettings.FlavorEnglish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEstonian)), nameof(ModSettings.FlavorEstonian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFaroese)), nameof(ModSettings.FlavorFaroese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFinnish)), nameof(ModSettings.FlavorFinnish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFrench)), nameof(ModSettings.FlavorFrench)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGerman)), nameof(ModSettings.FlavorGerman)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGreek)), nameof(ModSettings.FlavorGreek)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHebrew)), nameof(ModSettings.FlavorHebrew)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIcelandic)), nameof(ModSettings.FlavorIcelandic)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIndonesian)), nameof(ModSettings.FlavorIndonesian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorItalian)), nameof(ModSettings.FlavorItalian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorJapanese)), nameof(ModSettings.FlavorJapanese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorKorean)), nameof(ModSettings.FlavorKorean)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLatvian)), nameof(ModSettings.FlavorLatvian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLithuanian)), nameof(ModSettings.FlavorLithuanian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorNorwegian)), nameof(ModSettings.FlavorNorwegian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPolish)), nameof(ModSettings.FlavorPolish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPortuguese)), nameof(ModSettings.FlavorPortuguese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRomanian)), nameof(ModSettings.FlavorRomanian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRussian)), nameof(ModSettings.FlavorRussian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSerboCroatian)), nameof(ModSettings.FlavorSerboCroatian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovak)), nameof(ModSettings.FlavorSlovak)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovenian)), nameof(ModSettings.FlavorSlovenian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSpanish)), nameof(ModSettings.FlavorSpanish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSwedish)), nameof(ModSettings.FlavorSwedish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorThai)), nameof(ModSettings.FlavorThai)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorTurkish)), nameof(ModSettings.FlavorTurkish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorUkrainian)), nameof(ModSettings.FlavorUkrainian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorVietnamese)), nameof(ModSettings.FlavorVietnamese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseSimplified)), nameof(ModSettings.FlavorChineseSimplified)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseTraditional)), nameof(ModSettings.FlavorChineseTraditional)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHindi)), nameof(ModSettings.FlavorHindi)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHungarian)), nameof(ModSettings.FlavorHungarian)}

            };
    }

    public void Unload() { }
}