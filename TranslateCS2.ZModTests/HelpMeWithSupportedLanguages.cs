using TranslateCS2.ZModTests.TestHelpers;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;


namespace TranslateCS2.ZModTests;

public class HelpMeWithSupportedLanguages {
    public HelpMeWithSupportedLanguages() {
    }
    [Fact(Skip = "dont help me each time")]
    //[Fact()]
    public void GenerateJsons() {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<HelpMeWithSupportedLanguages>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(logProvider);
        string directoryPath = runtimeContainer.Paths.TryToGetModsPath();
        JSONGenerator generator = new JSONGenerator(directoryPath);
        generator.Generate(true, false);
    }
}
