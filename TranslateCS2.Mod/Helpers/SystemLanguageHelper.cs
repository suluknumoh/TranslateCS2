using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Game.SceneFlow;

using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod.Helpers;
internal class SystemLanguageHelper {
    public static SystemLanguageHelperResult Get(string id) {
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        IEnumerable<CultureInfo> cultureInfos =
            CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(item => item.Name.Equals(id, StringComparison.OrdinalIgnoreCase));
        foreach (CultureInfo cultureInfo in cultureInfos) {


            SystemLanguageHelperResult? result = TryToGetResult(systemLanguages, cultureInfo);
            if (result != null) {
                return result;
            }


        }
        return new SystemLanguageHelperResult(SystemLanguage.Unknown, null);
    }

    public static bool IsRandomizeLanguage(SystemLanguage? language) {
        string[] alreadySupportedLocales = GameManager.instance.localizationManager.GetSupportedLocales();
        foreach (string alreadySupportedLocale in alreadySupportedLocales) {
            SystemLanguage alreadyUsedSystemLanguage = GameManager.instance.localizationManager.LocaleIdToSystemLanguage(alreadySupportedLocale);
            if (alreadyUsedSystemLanguage == language) {
                return true;
            }
        }
        return false;
    }

    public static SystemLanguage Random() {
        IEnumerable<SystemLanguage> allSystemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        List<SystemLanguage> alreadyUsedSystemLanguages = [];
        string[] alreadySupportedLocales = GameManager.instance.localizationManager.GetSupportedLocales();
        foreach (string alreadySupportedLocale in alreadySupportedLocales) {
            SystemLanguage alreadyUsedSystemLanguage = GameManager.instance.localizationManager.LocaleIdToSystemLanguage(alreadySupportedLocale);
            alreadyUsedSystemLanguages.Add(alreadyUsedSystemLanguage);
        }
        if (alreadyUsedSystemLanguages.Count == allSystemLanguages.Count()) {
            throw new Exception("Too many Languages!");
        }
        SystemLanguage[] availableSystemLanguages = allSystemLanguages.Except(alreadyUsedSystemLanguages).ToArray();
        System.Random random = new System.Random();
        int index = random.Next(availableSystemLanguages.Count());
        return availableSystemLanguages[index];
    }

    private static SystemLanguageHelperResult? TryToGetResult(IEnumerable<SystemLanguage> systemLanguages, CultureInfo cultureInfo) {
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
                        return new SystemLanguageHelperResult(systemLanguage, cultureInfo);
                    }
                    break;
                default:
                    comparator = systemLanguage.ToString();
                    break;
            }
            if (cultureInfo.EnglishName.StartsWith(comparator, StringComparison.OrdinalIgnoreCase)) {
                return new SystemLanguageHelperResult(systemLanguage, cultureInfo);
            }
        }
        return null;
    }
}
