using System.Collections.Generic;
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
        if (RegExConstants.ContainsNonBasicLatinCharacters.IsMatch(cultureInfo.EnglishName)) {
            return EaseLocaleName(cultureInfo.Parent);
        }
        return cultureInfo.EnglishName;
    }
}
