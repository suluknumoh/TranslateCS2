using System;
using System.Globalization;

using UnityEngine;

namespace TranslateCS2.Mod.Helpers;
internal class SystemLanguageHelper {
    public static SystemLanguage Get(string id) {
        CultureInfo[] cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);
        foreach (CultureInfo cultureInfo in cultureInfos) {
            if (cultureInfo.Name.Equals(id, StringComparison.OrdinalIgnoreCase)) {
                Array systemLanguages = Enum.GetValues(typeof(SystemLanguage));
                foreach (SystemLanguage systemLanguage in systemLanguages) {
                    if (cultureInfo.EnglishName.StartsWith(systemLanguage.ToString(), StringComparison.OrdinalIgnoreCase)) {
                        return systemLanguage;
                    }
                }
            }
        }
        return SystemLanguage.Unknown;
    }
}
