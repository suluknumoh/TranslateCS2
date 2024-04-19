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
        Array systemLanguages = Enum.GetValues(typeof(SystemLanguage));
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

    public static SystemLanguage? Random() {
        Array availableSystemLanguages = Enum.GetValues(typeof(SystemLanguage));
        List<SystemLanguage> alreadyUsedSystemLanguages = [];
        string[] alreadySupportedLocales = GameManager.instance.localizationManager.GetSupportedLocales();
        foreach (string alreadySupportedLocale in alreadySupportedLocales) {
            SystemLanguage alreadyUsedSystemLanguage = GameManager.instance.localizationManager.LocaleIdToSystemLanguage(alreadySupportedLocale);
            alreadyUsedSystemLanguages.Add(alreadyUsedSystemLanguage);
        }
        if (alreadyUsedSystemLanguages.Count == availableSystemLanguages.Length) {
            throw new Exception("Too many Languages!");
        }
        System.Random random = new System.Random();
        while (true) {
            int index = random.Next(availableSystemLanguages.Length);
            SystemLanguage use = (SystemLanguage) availableSystemLanguages.GetValue(index);
            if (!alreadyUsedSystemLanguages.Contains(use)) {
                return use;
            }
        }
    }

    private static SystemLanguageHelperResult? TryToGetResult(Array systemLanguages, CultureInfo cultureInfo) {
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
