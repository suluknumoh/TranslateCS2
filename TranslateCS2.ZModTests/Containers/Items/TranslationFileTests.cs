using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal;

using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;
using TranslateCS2.ZZZTestLib.Services.Localizations;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
[Collection("TestDataOK")]
public class TranslationFileTests {
    private readonly TestDataProvider dataProvider;
    public TranslationFileTests(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
    }
    [Fact]
    public void ReadEntriesTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);

        string fallbackLocaleId = runtimeContainer.LocManager.FallbackLocaleId;


        TestLocFileServiceStrategy strategy = new TestLocFileServiceStrategy();
        LocFileService<string> locFileService = new LocFileService<string>(strategy);
        FileInfo enUS =
            locFileService
                .GetLocalizationFiles()
                .Where(item => item.Name.StartsWith(fallbackLocaleId))
                .First();

        MyLanguage? language = runtimeContainer.Languages.GetLanguage(fallbackLocaleId);
        Assert.NotNull(language);

        MyLocalization<string> locFile = locFileService.GetLocalizationFile(enUS);
        Assert.NotNull(locFile);
        Assert.True(locFile.IsOK);
        Assert.NotEmpty(locFile.Source.Localizations);
        Assert.NotEmpty(locFile.Source.IndexCounts);

        TranslationFile translationFile = new TranslationFile(runtimeContainer, language, locFile);
        Assert.True(translationFile.IsOK);
        Assert.NotEmpty(translationFile.Source.Localizations);
        Assert.NotEmpty(translationFile.Source.IndexCounts);

        List<IDictionaryEntryError> errors = [];
        Dictionary<string, int> indexCountsToFill = [];
        IEnumerable<KeyValuePair<string, string>> localizations = translationFile.ReadEntries(errors, indexCountsToFill);
        Assert.Empty(errors);

        Assert.NotEmpty(localizations);
        Assert.Equal(locFile.Source.Localizations, localizations);

        Assert.NotEmpty(indexCountsToFill);
        Assert.Equal(locFile.Source.IndexCounts, indexCountsToFill);
    }
}
