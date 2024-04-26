using System;
using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Properties.I18N;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private string Flavor { get; } = nameof(Flavor);
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


                // flavor-group-labels: test
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorAfrikaans)), this.GetLabel(nameof(ModSettings.FlavorAfrikaans))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorArabic)), this.GetLabel(nameof(ModSettings.FlavorArabic))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBasque)), this.GetLabel(nameof(ModSettings.FlavorBasque))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBelarusian)), this.GetLabel(nameof(ModSettings.FlavorBelarusian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBulgarian)), this.GetLabel(nameof(ModSettings.FlavorBulgarian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCatalan)), this.GetLabel(nameof(ModSettings.FlavorCatalan))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCzech)), this.GetLabel(nameof(ModSettings.FlavorCzech))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDanish)), this.GetLabel(nameof(ModSettings.FlavorDanish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDutch)), this.GetLabel(nameof(ModSettings.FlavorDutch))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEnglish)), this.GetLabel(nameof(ModSettings.FlavorEnglish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEstonian)), this.GetLabel(nameof(ModSettings.FlavorEstonian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFaroese)), this.GetLabel(nameof(ModSettings.FlavorFaroese))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFinnish)), this.GetLabel(nameof(ModSettings.FlavorFinnish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFrench)), this.GetLabel(nameof(ModSettings.FlavorFrench))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGerman)), this.GetLabel(nameof(ModSettings.FlavorGerman))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGreek)), this.GetLabel(nameof(ModSettings.FlavorGreek))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHebrew)), this.GetLabel(nameof(ModSettings.FlavorHebrew))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIcelandic)), this.GetLabel(nameof(ModSettings.FlavorIcelandic))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIndonesian)), this.GetLabel(nameof(ModSettings.FlavorIndonesian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorItalian)), this.GetLabel(nameof(ModSettings.FlavorItalian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorJapanese)), this.GetLabel(nameof(ModSettings.FlavorJapanese))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorKorean)), this.GetLabel(nameof(ModSettings.FlavorKorean))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLatvian)), this.GetLabel(nameof(ModSettings.FlavorLatvian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLithuanian)), this.GetLabel(nameof(ModSettings.FlavorLithuanian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorNorwegian)), this.GetLabel(nameof(ModSettings.FlavorNorwegian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPolish)), this.GetLabel(nameof(ModSettings.FlavorPolish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPortuguese)), this.GetLabel(nameof(ModSettings.FlavorPortuguese))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRomanian)), this.GetLabel(nameof(ModSettings.FlavorRomanian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRussian)), this.GetLabel(nameof(ModSettings.FlavorRussian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSerboCroatian)), this.GetLabel(nameof(ModSettings.FlavorSerboCroatian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovak)), this.GetLabel(nameof(ModSettings.FlavorSlovak))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovenian)), this.GetLabel(nameof(ModSettings.FlavorSlovenian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSpanish)), this.GetLabel(nameof(ModSettings.FlavorSpanish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSwedish)), this.GetLabel(nameof(ModSettings.FlavorSwedish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorThai)), this.GetLabel(nameof(ModSettings.FlavorThai))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorTurkish)), this.GetLabel(nameof(ModSettings.FlavorTurkish))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorUkrainian)), this.GetLabel(nameof(ModSettings.FlavorUkrainian))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorVietnamese)), this.GetLabel(nameof(ModSettings.FlavorVietnamese))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseSimplified)), this.GetLabel(nameof(ModSettings.FlavorChineseSimplified))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseTraditional)), this.GetLabel(nameof(ModSettings.FlavorChineseTraditional))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHindi)), this.GetLabel(nameof(ModSettings.FlavorHindi))},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHungarian)), this.GetLabel(nameof(ModSettings.FlavorHungarian))},


                // flavor-group-descriptions: test
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorAfrikaans)), this.GetDescription(nameof(ModSettings.FlavorAfrikaans))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorArabic)), this.GetDescription(nameof(ModSettings.FlavorArabic))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBasque)), this.GetDescription(nameof(ModSettings.FlavorBasque))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBelarusian)), this.GetDescription(nameof(ModSettings.FlavorBelarusian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBulgarian)), this.GetDescription(nameof(ModSettings.FlavorBulgarian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCatalan)), this.GetDescription(nameof(ModSettings.FlavorCatalan))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCzech)), this.GetDescription(nameof(ModSettings.FlavorCzech))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDanish)), this.GetDescription(nameof(ModSettings.FlavorDanish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDutch)), this.GetDescription(nameof(ModSettings.FlavorDutch))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEnglish)), this.GetDescription(nameof(ModSettings.FlavorEnglish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEstonian)), this.GetDescription(nameof(ModSettings.FlavorEstonian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFaroese)), this.GetDescription(nameof(ModSettings.FlavorFaroese))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFinnish)), this.GetDescription(nameof(ModSettings.FlavorFinnish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFrench)), this.GetDescription(nameof(ModSettings.FlavorFrench))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGerman)), this.GetDescription(nameof(ModSettings.FlavorGerman))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGreek)), this.GetDescription(nameof(ModSettings.FlavorGreek))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHebrew)), this.GetDescription(nameof(ModSettings.FlavorHebrew))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIcelandic)), this.GetDescription(nameof(ModSettings.FlavorIcelandic))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIndonesian)), this.GetDescription(nameof(ModSettings.FlavorIndonesian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorItalian)), this.GetDescription(nameof(ModSettings.FlavorItalian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorJapanese)), this.GetDescription(nameof(ModSettings.FlavorJapanese))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorKorean)), this.GetDescription(nameof(ModSettings.FlavorKorean))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLatvian)), this.GetDescription(nameof(ModSettings.FlavorLatvian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLithuanian)), this.GetDescription(nameof(ModSettings.FlavorLithuanian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorNorwegian)), this.GetDescription(nameof(ModSettings.FlavorNorwegian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPolish)), this.GetDescription(nameof(ModSettings.FlavorPolish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPortuguese)), this.GetDescription(nameof(ModSettings.FlavorPortuguese))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRomanian)), this.GetDescription(nameof(ModSettings.FlavorRomanian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRussian)), this.GetDescription(nameof(ModSettings.FlavorRussian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSerboCroatian)), this.GetDescription(nameof(ModSettings.FlavorSerboCroatian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovak)), this.GetDescription(nameof(ModSettings.FlavorSlovak))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovenian)), this.GetDescription(nameof(ModSettings.FlavorSlovenian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSpanish)), this.GetDescription(nameof(ModSettings.FlavorSpanish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSwedish)), this.GetDescription(nameof(ModSettings.FlavorSwedish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorThai)), this.GetDescription(nameof(ModSettings.FlavorThai))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorTurkish)), this.GetDescription(nameof(ModSettings.FlavorTurkish))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorUkrainian)), this.GetDescription(nameof(ModSettings.FlavorUkrainian))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorVietnamese)), this.GetDescription(nameof(ModSettings.FlavorVietnamese))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseSimplified)), this.GetDescription(nameof(ModSettings.FlavorChineseSimplified))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseTraditional)), this.GetDescription(nameof(ModSettings.FlavorChineseTraditional))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHindi)), this.GetDescription(nameof(ModSettings.FlavorHindi))},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHungarian)), this.GetDescription(nameof(ModSettings.FlavorHungarian))}

            };
    }

    private string GetLabel(string s) {
        string param = s.Replace(this.Flavor, String.Empty);
        string messagePre = I18NMod.FlavorLabel;
        return String.Format(messagePre, param);
    }
    private string GetDescription(string s) {
        string param = s.Replace(this.Flavor, String.Empty);
        string messagePre = I18NMod.FlavorDescription;
        return String.Format(messagePre, param);
    }

    public void Unload() { }
}