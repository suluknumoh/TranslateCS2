using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Colossal.PSI.Environment;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Helpers;
internal static class LocaleHelper {
    private static IReadOnlyDictionary<string, string> LowerCaseToBuiltIn { get; }
    static LocaleHelper() {
        Dictionary<string, string> dictionary = [];
        // has to end with a forward-slash
        string path = $"{EnvPath.kStreamingDataPath}/Data~/";
        IEnumerable<string> locFiles = Directory.EnumerateFiles(path, ModConstants.LocSearchPattern);
        foreach (string? locFile in locFiles) {
            string locale =
                locFile
                .Replace(path, String.Empty)
                .Replace(ModConstants.LocExtension, String.Empty);
            dictionary.Add(locale.ToLower(), locale);
        }
        LowerCaseToBuiltIn = dictionary;
    }
    public static string CorrectLocaleId(string localeId) {
        if (LowerCaseToBuiltIn.TryGetValue(localeId.ToLower(), out string? ret) && ret != null) {
            return ret;
        }
        IEnumerable<CultureInfo> cis = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (cis.Any()) {
            return cis.First().Name;
        }
        return localeId;
    }
    public static bool IsBuiltIn(string localeId) {
        return LowerCaseToBuiltIn.ContainsKey(localeId.ToLower());
    }
}
