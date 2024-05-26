using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;


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
        JSONGenerator generator = new JSONGenerator(directoryPath);
        generator.Generate(true, false);
    }
}
