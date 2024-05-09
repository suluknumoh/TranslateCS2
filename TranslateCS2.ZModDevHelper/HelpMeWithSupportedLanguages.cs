using System.IO;
using System.Text;

using TranslateCS2.Inf;


namespace TranslateCS2.ZModDevHelper;

public class HelpMeWithSupportedLanguages {
    public HelpMeWithSupportedLanguages() {
    }
    [Fact(Skip = "dont help me each time")]
    //[Fact()]
    public void GenerateJsons() {
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer();
        string directoryPath = runtimeContainer.Paths.TryToGetModsPath();
        foreach (string supportedLocale in LocalesSupported.Lowered) {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            // to see, if changes are applied on startup
            builder.AppendLine($"\"Menu.OPTIONS\": \"Options ({supportedLocale}.json)\",");
            // to see, if changes are applied on flavor changed
            builder.AppendLine($"\"Options.SECTION[General]\": \"General ({supportedLocale}.json)\",");
            builder.AppendLine("}");
            string filePath = Path.Combine(directoryPath, $"{supportedLocale}{ModConstants.JsonExtension}");
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, builder.ToString(), Encoding.UTF8);
        }
    }
}
