using System.IO;
using System.Reflection;
using System.Text;

using TranslateCS2.Configurations;

namespace TranslateCS2.Helpers;
internal class MarkdownHelper {
    private static readonly string _readmePath = $"{AppConfigurationManager.AssetPath}README.md";
    public static string GetReadmeTillCaption(string caption) {
        Assembly assembly = Assembly.GetExecutingAssembly();
        StringBuilder builder = new StringBuilder();
        try {
            using Stream? stream = assembly.GetManifestResourceStream(_readmePath);
            if (stream != null) {
                using StreamReader sr = new StreamReader(stream);
                string? line = null;
                while ((line = sr.ReadLine()) != null) {
                    if (line == caption) {
                        break;
                    }
                    builder.AppendLine(line);
                }
            }
        } catch {
            AppendErrorMessage(builder);
        }
        return builder.ToString();
    }
    public static string GetReadmeFromCaption(string caption) {
        Assembly assembly = Assembly.GetExecutingAssembly();
        StringBuilder builder = new StringBuilder();
        try {
            using Stream? stream = assembly.GetManifestResourceStream(_readmePath);
            if (stream != null) {
                using StreamReader sr = new StreamReader(stream);
                string? line = null;
                bool start = false;
                while ((line = sr.ReadLine()) != null) {
                    if (line == caption) {
                        start = true;
                    }
                    if (start) {
                        builder.AppendLine(line);
                    }
                }
            }
        } catch {
            AppendErrorMessage(builder);
        }
        return builder.ToString();
    }

    private static void AppendErrorMessage(StringBuilder builder) {
        builder.AppendLine("# Error");
        builder.AppendLine("Couldn't read readme.md!");
    }
}
