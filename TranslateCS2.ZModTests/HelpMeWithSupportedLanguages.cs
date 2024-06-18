using System.Collections.Generic;
using System.Globalization;
using System.IO;

using TranslateCS2.Inf;
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
        if (false) {
            Directory.CreateDirectory(directoryPath);
            List<CultureInfo> supported = CultureInfoHelper.GetSupportedCultures();
            foreach (CultureInfo supportedLocale in supported) {
                string filePath = Path.Combine(directoryPath, $"{supportedLocale.Name.ToLower()}{ModConstants.JsonExtension}");
                File.Copy("E:\\cs2\\zzz_origin_fr.json", filePath, true);
            }
            return;
        }
        JSONGenerator generator = new JSONGenerator(directoryPath, 0);
        generator.Generate(true, false);
    }
}
