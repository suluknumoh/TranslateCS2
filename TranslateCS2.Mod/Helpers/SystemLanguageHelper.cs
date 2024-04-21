using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Colossal.Localization;

using Game.SceneFlow;

using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod.Helpers;
internal class SystemLanguageHelper {
    private static readonly LocalizationManager LocalizationManager = GameManager.instance.localizationManager;
    public static SystemLanguageCultureInfo Get(string id) {
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        IEnumerable<CultureInfo> cultureInfos =
            CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(item => item.Name.Equals(id, StringComparison.OrdinalIgnoreCase));
        foreach (CultureInfo cultureInfo in cultureInfos) {


            SystemLanguageCultureInfo? result = TryToGetResult(systemLanguages, cultureInfo);
            if (result != null) {
                return result;
            }


        }
        return new SystemLanguageCultureInfo(SystemLanguage.Unknown, null);
    }

    public static bool IsSystemLanguageInUse(SystemLanguage? language) {
        string[] alreadySupportedLocales = LocalizationManager.GetSupportedLocales();
        foreach (string alreadySupportedLocale in alreadySupportedLocales) {
            SystemLanguage alreadyUsedSystemLanguage = LocalizationManager.LocaleIdToSystemLanguage(alreadySupportedLocale);
            if (alreadyUsedSystemLanguage == language) {
                return true;
            }
        }
        return false;
    }

    private static SystemLanguageCultureInfo? TryToGetResult(IEnumerable<SystemLanguage> systemLanguages, CultureInfo cultureInfo) {
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            string? comparator = null;
            switch (systemLanguage) {
                case SystemLanguage.ChineseSimplified:
                    // INFO: SystemLanguage.ChineseSimplified -> in CultureInfo its "Chinese (Simplified..."
                    comparator = "Chinese (Simplified";
                    break;
                case SystemLanguage.ChineseTraditional:
                    // INFO: SystemLanguage.ChineseTraditional -> in CultureInfo its "Chinese (Traditional..."
                    comparator = "Chinese (Traditional";
                    break;
                case SystemLanguage.SerboCroatian:
                    // INFO: SystemLanguage.SerboCroatian -> in CultureInfo its separated
                    if (cultureInfo.EnglishName.StartsWith("Croatian", StringComparison.OrdinalIgnoreCase)
                        || cultureInfo.EnglishName.StartsWith("Serbian", StringComparison.OrdinalIgnoreCase)) {
                        return new SystemLanguageCultureInfo(systemLanguage, cultureInfo);
                    }
                    break;
                default:
                    comparator = systemLanguage.ToString();
                    break;
            }
            if (cultureInfo.EnglishName.StartsWith(comparator, StringComparison.OrdinalIgnoreCase)) {
                return new SystemLanguageCultureInfo(systemLanguage, cultureInfo);
            }
        }
        return null;
    }
}
