using System.IO;
using System.Reflection;
using System.Text;

namespace TranslateCS2.Core.Helpers;
public static class MarkDownHelper {
    public static string GetMarkDown(Assembly? assembly, string resourceName) {
        try {
            using Stream? stream = assembly?.GetManifestResourceStream(resourceName);
            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        } catch {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("# Error");
            builder.AppendLine($"unable to read internal resource: {resourceName}");
            return builder.ToString();
        }
    }
}
