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
            string content = $"{{ \"Options.SECTION[General]\": \"General ({supportedLocale}.json)\" }}";
            string filePath = Path.Combine(directoryPath, $"{supportedLocale}{ModConstants.JsonExtension}");
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            File.WriteAllText(filePath, content, Encoding.UTF8);
        }
    }
}
