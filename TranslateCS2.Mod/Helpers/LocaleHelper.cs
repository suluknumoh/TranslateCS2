using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using Colossal.PSI.Environment;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Helpers;
internal static class LocaleHelper {
    public static IList<string> BuiltIn { get; } = [];
    static LocaleHelper() {
        // INFO: is it ok?
        // has to end with a forward-slash
        string path = $"{EnvPath.kStreamingDataPath}/Data~/";
        IEnumerable<string> locFiles = Directory.EnumerateFiles(path, ModConstants.LocSearchPattern);
        foreach (string? locFile in locFiles) {
            string locale =
                locFile
                .Replace(path, String.Empty)
                .Replace(ModConstants.LocExtension, String.Empty);
            BuiltIn.Add(locale);
        }
    }

    public static string EaseLocaleName(CultureInfo cultureInfo) {
        if (RegExConstants.ContainsNonBasicLatinCharacters.IsMatch(cultureInfo.NativeName)) {
            return cultureInfo.EnglishName;
        }
        return cultureInfo.NativeName;
    }
}
