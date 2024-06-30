using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Enums;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;
using TranslateCS2.ZZZTestLib.Services.Localizations;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class FlavorTests : AProvidesTestDataOk {
    public FlavorTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void ReadEntriesTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<FlavorTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
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

        FlavorSource flavorSource = new FlavorSource(FlavorSourceTypes.THIS,
                                                     locFile,
                                                     ModConstants.NameSimple,
                                                     ModConstants.ModId);
        Assert.True(flavorSource.IsOk);
        Assert.NotEmpty(flavorSource.Source.Localizations);
        Assert.NotEmpty(flavorSource.Source.IndexCounts);

        Flavor flavor = new Flavor(runtimeContainer, language, locFile.Id, locFile.Name, locFile.NameEnglish);
        flavor.FlavorSources.Add(flavorSource);

        List<IDictionaryEntryError> errors = [];
        Dictionary<string, int> indexCountsToFill = [];
        IEnumerable<KeyValuePair<string, string>> localizations = flavor.ReadEntries(errors, indexCountsToFill);
        Assert.Empty(errors);

        Assert.NotEmpty(localizations);
        Assert.Equal(locFile.Source.Localizations, localizations);

        Assert.NotEmpty(indexCountsToFill);
        Assert.Equal(locFile.Source.IndexCounts, indexCountsToFill);
    }
    [Fact]
    public void AddLocaleNameLocalizedKeyTest() {
        TestDataProvider dataProviderLocal = new TestDataProvider {
            DirectoryName = nameof(AddLocaleNameLocalizedKeyTest)
        };
        try {
            dataProviderLocal.GenerateData(true, true);
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<FlavorTests>();
            ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                      userDataPath: dataProviderLocal.DirectoryName);
            runtimeContainer.Init();
            MyLanguages myLanguages = runtimeContainer.Languages;
            IEnumerable<MyLanguage> languages = myLanguages.LanguageDictionary.Values;
            foreach (MyLanguage language in languages) {
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    // generate data adds the "LocaleNameLocalizedKey"
                    // with the flavor files name (without path and without extension)
                    // to the testdata
                    // what should cause the localized name to be equal to the id
                    if (language.IsBuiltIn) {
                        // ignore case for built-in
                        // example: zh-HANT vs zh-Hant
                        Assert.Equal(flavor.Id, flavor.Name, true);
                    } else {
                        Assert.Equal(flavor.Id, flavor.Name);
                    }
                }
            }
        } finally {
            dataProviderLocal.Dispose();
        }
    }
}
