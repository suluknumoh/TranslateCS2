using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TranslateCS2.ModBridge;
public static class CultureInfoHelper {
    public static IEnumerable<CultureInfo>? GatherCulturesFromEnglishName(string? englishName) {
        if (englishName == null) {
            return null;
        }
        // mscorlib used by co supports less cultureinfos
        IEnumerable<CultureInfo> specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures).Where(item => LocalesSupported.IsLocaleIdSupported(item.Name));
        IEnumerable<CultureInfo> guessedCultures =
            specificCultures.Where(
                item =>
                    item.EnglishName.StartsWith(englishName, StringComparison.OrdinalIgnoreCase)
                    || englishName.ToLower().Contains(item.EnglishName.ToLower())
        );
        IOrderedEnumerable<CultureInfo> orderedGuessedCultures = guessedCultures.OrderBy(item => item.Name);
        if (orderedGuessedCultures.Any()) {
            return orderedGuessedCultures;
        }
        return null;
    }
}
