using System.Collections.Generic;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Properties.I18N;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class ModSettingsLocale : IDictionarySource {
    private static MyLanguages Languages { get; } = MyLanguages.Instance;
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
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorAfrikaans)), this.GetLabel(SystemLanguage.Afrikaans)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorArabic)), this.GetLabel(SystemLanguage.Arabic)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBasque)), this.GetLabel(SystemLanguage.Basque)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBelarusian)), this.GetLabel(SystemLanguage.Belarusian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorBulgarian)), this.GetLabel(SystemLanguage.Bulgarian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCatalan)), this.GetLabel(SystemLanguage.Catalan)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorCzech)), this.GetLabel(SystemLanguage.Czech)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDanish)), this.GetLabel(SystemLanguage.Danish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorDutch)), this.GetLabel(SystemLanguage.Dutch)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEnglish)), this.GetLabel(SystemLanguage.English)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorEstonian)), this.GetLabel(SystemLanguage.Estonian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFaroese)), this.GetLabel(SystemLanguage.Faroese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFinnish)), this.GetLabel(SystemLanguage.Finnish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorFrench)), this.GetLabel(SystemLanguage.French)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGerman)), this.GetLabel(SystemLanguage.German)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorGreek)), this.GetLabel(SystemLanguage.Greek)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHebrew)), this.GetLabel(SystemLanguage.Hebrew)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHungarian)), this.GetLabel(SystemLanguage.Hungarian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIcelandic)), this.GetLabel(SystemLanguage.Icelandic)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorIndonesian)), this.GetLabel(SystemLanguage.Indonesian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorItalian)), this.GetLabel(SystemLanguage.Italian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorJapanese)), this.GetLabel(SystemLanguage.Japanese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorKorean)), this.GetLabel(SystemLanguage.Korean)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLatvian)), this.GetLabel(SystemLanguage.Latvian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorLithuanian)), this.GetLabel(SystemLanguage.Lithuanian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorNorwegian)), this.GetLabel(SystemLanguage.Norwegian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPolish)), this.GetLabel(SystemLanguage.Polish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorPortuguese)), this.GetLabel(SystemLanguage.Portuguese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRomanian)), this.GetLabel(SystemLanguage.Romanian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorRussian)), this.GetLabel(SystemLanguage.Russian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSerboCroatian)), this.GetLabel(SystemLanguage.SerboCroatian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovak)), this.GetLabel(SystemLanguage.Slovak)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSlovenian)), this.GetLabel(SystemLanguage.Slovenian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSpanish)), this.GetLabel(SystemLanguage.Spanish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorSwedish)), this.GetLabel(SystemLanguage.Swedish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorThai)), this.GetLabel(SystemLanguage.Thai)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorTurkish)), this.GetLabel(SystemLanguage.Turkish)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorUkrainian)), this.GetLabel(SystemLanguage.Ukrainian)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorVietnamese)), this.GetLabel(SystemLanguage.Vietnamese)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseSimplified)), this.GetLabel(SystemLanguage.ChineseSimplified)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorChineseTraditional)), this.GetLabel(SystemLanguage.ChineseTraditional)},
                { this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.FlavorHindi)), this.GetLabel(SystemLanguage.Hindi)},




                // flavor-group-descriptions: test
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorAfrikaans)), this.GetDescription(SystemLanguage.Afrikaans)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorArabic)), this.GetDescription(SystemLanguage.Arabic)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBasque)), this.GetDescription(SystemLanguage.Basque)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBelarusian)), this.GetDescription(SystemLanguage.Belarusian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorBulgarian)), this.GetDescription(SystemLanguage.Bulgarian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCatalan)), this.GetDescription(SystemLanguage.Catalan)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorCzech)), this.GetDescription(SystemLanguage.Czech)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDanish)), this.GetDescription(SystemLanguage.Danish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorDutch)), this.GetDescription(SystemLanguage.Dutch)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEnglish)), this.GetDescription(SystemLanguage.English)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorEstonian)), this.GetDescription(SystemLanguage.Estonian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFaroese)), this.GetDescription(SystemLanguage.Faroese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFinnish)), this.GetDescription(SystemLanguage.Finnish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorFrench)), this.GetDescription(SystemLanguage.French)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGerman)), this.GetDescription(SystemLanguage.German)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorGreek)), this.GetDescription(SystemLanguage.Greek)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHebrew)), this.GetDescription(SystemLanguage.Hebrew)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHungarian)), this.GetDescription(SystemLanguage.Hungarian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIcelandic)), this.GetDescription(SystemLanguage.Icelandic)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorIndonesian)), this.GetDescription(SystemLanguage.Indonesian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorItalian)), this.GetDescription(SystemLanguage.Italian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorJapanese)), this.GetDescription(SystemLanguage.Japanese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorKorean)), this.GetDescription(SystemLanguage.Korean)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLatvian)), this.GetDescription(SystemLanguage.Latvian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorLithuanian)), this.GetDescription(SystemLanguage.Lithuanian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorNorwegian)), this.GetDescription(SystemLanguage.Norwegian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPolish)), this.GetDescription(SystemLanguage.Polish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorPortuguese)), this.GetDescription(SystemLanguage.Portuguese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRomanian)), this.GetDescription(SystemLanguage.Romanian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorRussian)), this.GetDescription(SystemLanguage.Russian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSerboCroatian)), this.GetDescription(SystemLanguage.SerboCroatian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovak)), this.GetDescription(SystemLanguage.Slovak)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSlovenian)), this.GetDescription(SystemLanguage.Slovenian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSpanish)), this.GetDescription(SystemLanguage.Spanish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorSwedish)), this.GetDescription(SystemLanguage.Swedish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorThai)), this.GetDescription(SystemLanguage.Thai)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorTurkish)), this.GetDescription(SystemLanguage.Turkish)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorUkrainian)), this.GetDescription(SystemLanguage.Ukrainian)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorVietnamese)), this.GetDescription(SystemLanguage.Vietnamese)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseSimplified)), this.GetDescription(SystemLanguage.ChineseSimplified)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorChineseTraditional)), this.GetDescription(SystemLanguage.ChineseTraditional)},
                { this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.FlavorHindi)), this.GetDescription(SystemLanguage.Hindi)},



            };
    }

    private string GetLabel(SystemLanguage systemLanguage) {
        string param = this.GetLanguageName(systemLanguage);
        string messagePre = I18NMod.FlavorLabel;
        return $"{messagePre} {param}";
    }
    private string GetDescription(SystemLanguage systemLanguage) {
        string param = this.GetLanguageName(systemLanguage);
        string messagePre = I18NMod.FlavorDescription;
        return $"{messagePre} {param}";
    }
    private string GetLanguageName(SystemLanguage systemLanguage) {
        MyLanguage? language = Languages.GetLanguage(systemLanguage);
        if (language == null) {
            return systemLanguage.ToString();
        }
        return language.NameEnglish;
    }

    public void Unload() { }
}
