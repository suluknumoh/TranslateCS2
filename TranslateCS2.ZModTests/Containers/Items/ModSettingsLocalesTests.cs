using System.Collections.Generic;
using System.Linq;

using Colossal;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsLocalesTests : AProvidesTestDataOk {
    private readonly int expectedLocaleSourcesCount = 12;


    private readonly Dictionary<string, int> expectedCountsPerSource = new Dictionary<string, int>() {
        {"en-US", 30 },
        //
        {"de-DE", 0 },
        {"es-ES", 0 },
        {"fr-FR", 0 },
        {"it-IT", 0 },
        {"ja-JP", 0 },
        {"ko-KR", 0 },
        {"pl-PL", 0 },
        {"pt-BR", 0 },
        {"ru-RU", 0 },
        {"zh-HANS", 0 },
        {"zh-HANT", 0 },
    };



    public ModSettingsLocalesTests(TestDataProvider testDataProvider) : base(testDataProvider) { }


    [Fact]
    public void GeneralTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsLocalesTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        LocManager locManager = runtimeContainer.LocManager;
        string fallBackLocaleId = locManager.FallbackLocaleId;
        ModSettingsLocales settingsLocale = modSettings.SettingsLocale;
        Assert.NotEmpty(settingsLocale.AllEntries);
        Assert.NotEmpty(settingsLocale.ExportableEntries);
        Assert.NotEqual(settingsLocale.AllEntries, settingsLocale.ExportableEntries);
        Assert.Equal(this.expectedLocaleSourcesCount, settingsLocale.LocaleSources.Count);
        foreach (ModSettingsLocaleSource localeSource in settingsLocale.LocaleSources.Values) {
            IList<IDictionaryEntryError> errors = [];
            Dictionary<string, int> indexCounts = [];
            IEnumerable<KeyValuePair<string, string>> readEntries = localeSource.ReadEntries(errors, indexCounts);
            Assert.Empty(errors);
            Assert.Empty(indexCounts);
            int expected = this.expectedCountsPerSource[localeSource.LocaleId];
            Assert.Equal(expected, readEntries.Count());
            if (localeSource.LocaleId.Equals(fallBackLocaleId)) {
                Assert.True(localeSource.IsFallBack);
                Assert.Equal(expected - 2, localeSource.ExportableEntries.Count);
                Assert.Equal(expected, localeSource.AllEntries.Count);
            } else {
                Assert.False(localeSource.IsFallBack);
                Assert.Equal(expected, localeSource.AllEntries.Count);
                Assert.Empty(localeSource.ExportableEntries);
            }

        }
    }
}
