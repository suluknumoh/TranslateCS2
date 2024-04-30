using System;
using System.IO;

namespace TranslateCS2.Inf;
public static class PathHelper {
    public static string? TryToGetModsPath() {
        string general = Path.Combine($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}Low",
                                      "Colossal Order",
                                      "Cities Skylines II",
                                      ModConstants.ModsData);
        if (Directory.Exists(general)) {
            string specific = Path.Combine(general, ModConstants.Name);
            if (Directory.Exists(specific)) {
                return specific;
            }
            return general;
        }
        return null;
    }
}
