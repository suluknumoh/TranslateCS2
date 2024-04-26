﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TranslateCS2.ModBridge;
public static class CultureInfoHelper {
    public static CultureInfo? GatherCultureFromEnglishName(string? englishName) {
        if (englishName == null) {
            return null;
        }
        // mscorlib used by co supports less cultureinfos
        IEnumerable<CultureInfo> specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(item => LocalesSupported.List.Contains(item.Name));
        IEnumerable<CultureInfo> guessedCultures = specificCultures.Where(item => item.EnglishName.StartsWith(englishName, System.StringComparison.OrdinalIgnoreCase));
        IOrderedEnumerable<CultureInfo> orderedGuessedCultures = guessedCultures.OrderBy(item => item.Name);
        if (orderedGuessedCultures.Any()) {
            return orderedGuessedCultures.First();
        }
        return null;
    }
    public static string EaseLocaleName(CultureInfo cultureInfo) {
        string? ret = EaseLocaleNameNative(cultureInfo, 2);
        ret ??= EaseLocaleNameEnglish(cultureInfo, 2);
        return ret;
    }
    private static string EaseLocaleNameEnglish(CultureInfo cultureInfo, int steps) {
        if (RegExConstants.ContainsNonBasicLatinCharacters.IsMatch(cultureInfo.EnglishName)) {
            if (steps <= 0) {
                return cultureInfo.Name;
            }
            return EaseLocaleNameEnglish(cultureInfo.Parent, --steps);
        }
        return cultureInfo.EnglishName;
    }
    private static string? EaseLocaleNameNative(CultureInfo cultureInfo, int steps) {
        if (RegExConstants.ContainsNonBasicLatinCharacters.IsMatch(cultureInfo.NativeName)) {
            if (steps <= 0) {
                return null;
            }
            return EaseLocaleNameNative(cultureInfo.Parent, --steps);
        }
        return cultureInfo.NativeName;
    }
}
