using System.IO;
using System.Text;

using TranslateCS2.Inf;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;


namespace TranslateCS2.ZModDevHelper;

public class HelpMeWithSupportedLanguages {
    public HelpMeWithSupportedLanguages() {
    }
    [Fact(Skip = "dont help me each time")]
    //[Fact()]
    public void GenerateJsons() {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<HelpMeWithSupportedLanguages>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(logProvider);
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
