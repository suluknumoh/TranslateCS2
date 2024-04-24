using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal.Localization;
using Colossal.PSI.Environment;

using Game.SceneFlow;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Helpers;
internal static class LocaleHelper {
    private static readonly LocalizationManager LocalizationManager = GameManager.instance.localizationManager;
    public static IList<string> BuiltIn { get; } = [];
    static LocaleHelper() {
        // TODO: is it ok?
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
    public static bool MapsToExisting(string localeId) {
        HashSet<string> set = LocalizationManager.GetSupportedLocales().Select(i => i.Split('-')[0]).ToHashSet();
        return set.Contains(localeId.Split('-')[0]);
    }

    public static string GetExisting(string localeId) {
        string id = $"{localeId.Split('-')[0]}-";
        IEnumerable<string> list = LocalizationManager.GetSupportedLocales().Where(item => item.StartsWith(id, StringComparison.OrdinalIgnoreCase));
        if (!list.Any()) {
            return localeId;
        }
        return list.First();
    }
}
