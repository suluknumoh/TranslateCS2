using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Core.Helpers;
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
}
