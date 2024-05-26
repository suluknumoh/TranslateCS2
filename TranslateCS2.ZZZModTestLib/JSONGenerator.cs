using System.IO;
using System.Text;

using TranslateCS2.Inf;

namespace TranslateCS2.ZZZModTestLib;
public static class JSONGenerator {
    /// <summary>
    ///     generates localization JSONs in the given <paramref name="destination"/>
    /// </summary>
    /// <param name="destination">
    ///     path to where the JSONs are generated
    ///     <br/>
    ///     <br/>
    ///     gets created if it does <see langword="not"/>exist
    /// </param>
    /// <param name="ok">
    ///     <see langword="true"/> (<see langword="default"/>) - generates NON corrupt data
    ///     <br/>
    ///     <see langword="false"/> (has to be set explicitly) - generates corrupt data
    /// </param>
    /// <param name="overwrite">
    ///     <see langword="true"/> (<see langword="default"/>) - overwrites existing data
    ///     <br/>
    ///     <see langword="false"/> (has to be set explicitly) - does not overwrite existing data
    /// </param>
    /// <returns>
    ///     the amount of entries in a single file
    /// </returns>
    public static int Generate(string destination,
                               bool ok = true,
                               bool overwrite = true) {
        Directory.CreateDirectory(destination);
        int entryCountPerFile = 0;
        string colon = "";
        if (ok) {
            colon = ":";
        }
        foreach (string supportedLocale in LocalesSupported.Lowered) {
            string filePath = Path.Combine(destination, $"{supportedLocale}{ModConstants.JsonExtension}");
            if (File.Exists(filePath)
                && !overwrite) {
                continue;
            }
            int count = 0;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            // to see, if changes are applied on startup
            AppendEntry(builder,
                        $"\"Menu.OPTIONS\"{colon} \"Options ({supportedLocale}.json)\",",
                        ref count);
            // to see, if changes are applied on flavor changed
            AppendEntry(builder,
                        $"\"Options.SECTION[General]\"{colon} \"General ({supportedLocale}.json)\",",
                        ref count);
            builder.AppendLine("}");
            File.WriteAllText(filePath,
                              builder.ToString(),
                              Encoding.UTF8);
            entryCountPerFile = count;
        }
        return entryCountPerFile;
    }
    private static void AppendEntry(StringBuilder builder,
                                    string entry,
                                    ref int count) {
        builder.AppendLine(entry);
        count++;
    }
}
