using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TranslateCS2.Core.Helpers;
public static class CultureInfoHelper {
    public static CultureInfo? GatherCultureFromEnglishName(string? englishName) {
        if (englishName == null) {
            return null;
        }
        CultureInfo[] specificCultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        IEnumerable<CultureInfo> guessedCultures = specificCultures.Where(item => item.EnglishName.StartsWith(englishName));
        IOrderedEnumerable<CultureInfo> orderedGuessedCultures = guessedCultures.OrderBy(item => item.Name);
        if (orderedGuessedCultures.Any()) {
            return orderedGuessedCultures.First();
        }
        return null;
    }
}
