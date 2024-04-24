using System;
using System.IO;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Core.Helpers;
public static class PathHelper {
    public static string? TryToGetModsPath() {
        string general = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low",
                                      "Colossal Order",
                                      "Cities Skylines II",
                                      ModConstants.ModsData);
        if (Path.Exists(general)) {
            string specific = Path.Combine(general, ModConstants.Name);
            if (Path.Exists(specific)) {
                return specific;
            }
            return general;
        }
        return null;
    }
}
