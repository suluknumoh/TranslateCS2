using System;
using System.IO;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Core.Helpers;
public static class PathHelper {
    public static string? TryToGetModsPath() {
        string general = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low",
                                           "Colossal Order",
                                           "Cities Skylines II",
                                           ".cache",
                                           "Mods",
                                           "mods_subscribed");
        if (Path.Exists(general)) {
            string specific = Path.Combine(general, $"{ModConstants.ModId}_1");
            if (Path.Exists(specific)) {
                return specific;
            }
            return general;
        }
        return null;
    }
}
