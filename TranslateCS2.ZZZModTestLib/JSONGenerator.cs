using System.IO;
using System.Text;

using TranslateCS2.Inf;

namespace TranslateCS2.ZZZModTestLib;
public static class JSONGenerator {
    public static void Generate(string destination,
                                bool ok = true,
                                bool overwrite = true) {
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
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            // to see, if changes are applied on startup
            builder.AppendLine($"\"Menu.OPTIONS\"{colon} \"Options ({supportedLocale}.json)\",");
            // to see, if changes are applied on flavor changed
            builder.AppendLine($"\"Options.SECTION[General]\"{colon} \"General ({supportedLocale}.json)\",");
            builder.AppendLine("}");
            File.WriteAllText(filePath,
                              builder.ToString(),
                              Encoding.UTF8);
        }
    }
}
