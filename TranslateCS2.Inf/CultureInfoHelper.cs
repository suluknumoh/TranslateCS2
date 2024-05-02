using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TranslateCS2.Inf;
public static class CultureInfoHelper {
    public static IEnumerable<CultureInfo> GetSupportedCultures() {
        return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(item => LocalesSupported.IsLocaleIdSupported(item.Name));
    }
    public static IEnumerable<CultureInfo>? GatherCulturesFromEnglishName(string? englishName) {
        if (englishName == null) {
            return null;
        }
        // mscorlib used by co supports less cultureinfos
        IEnumerable<CultureInfo> allCultures = GetSupportedCultures();
        IEnumerable<CultureInfo> guessedCultures =
            allCultures.Where(
                item =>
                    item.EnglishName.StartsWith(englishName, StringComparison.OrdinalIgnoreCase)
                    || englishName.ToLower().Contains(item.EnglishName.ToLower())
            );
        if (guessedCultures.Any()) {
            CultureInfo? ci = null;
            if (HasNeutralCultures(guessedCultures)) {
                ci = GetNeutralCultures(guessedCultures).First();
            } else if (HasSpecificCultures(guessedCultures)) {
                ci = GetSpecificCultures(guessedCultures).First().Parent;
            }
            if (ci != null) {
                string name = ci.Name;
                if (name.StartsWith(LangConstants.ChineseSimplified)) {
                    name = LangConstants.ChineseSimplified;
                } else if (name.StartsWith(LangConstants.ChineseTraditional)) {
                    name = LangConstants.ChineseTraditional;
                }
                guessedCultures = allCultures.Where(item => item.Name.StartsWith(name));
            }
        }
        IOrderedEnumerable<CultureInfo> orderedGuessedCultures = guessedCultures.OrderBy(item => item.Name);
        if (orderedGuessedCultures.Any()) {
            return orderedGuessedCultures;
        }
        return null;
    }
    public static bool HasNeutralCultures(IEnumerable<CultureInfo> cultures) {
        return HasCultures(cultures, CultureTypes.NeutralCultures);
    }
    public static bool HasSpecificCultures(IEnumerable<CultureInfo> cultures) {
        return HasCultures(cultures, CultureTypes.SpecificCultures);
    }
    public static bool HasCultures(IEnumerable<CultureInfo> cultures, CultureTypes cultureTypes) {
        return GetCulturesByTypes(cultures, cultureTypes).Any();
    }
    public static IEnumerable<CultureInfo> GetNeutralCultures(IEnumerable<CultureInfo> cultures) {
        return GetCulturesByTypes(cultures, CultureTypes.NeutralCultures);
    }
    public static IEnumerable<CultureInfo> GetSpecificCultures(IEnumerable<CultureInfo> cultures) {
        return GetCulturesByTypes(cultures, CultureTypes.SpecificCultures);
    }
    public static IEnumerable<CultureInfo> GetCulturesByTypes(IEnumerable<CultureInfo> cultures, CultureTypes cultureTypes) {
        return cultures.Where(item => (item.CultureTypes & cultureTypes) == cultureTypes);
    }
}
