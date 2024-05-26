using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Models;
using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
[Collection("TestDataOK")]
public class ModSettingsLocaleTests {
    private readonly TestDataProvider dataProvider;
    public ModSettingsLocaleTests(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
    }
    [Fact]
    public void GeneralTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsLocaleTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        ModSettingsLocale settingsLocale = runtimeContainer.SettingsLocale;
        Assert.NotEmpty(settingsLocale.AllEntries);
        Assert.NotEmpty(settingsLocale.ExportableEntries);
        Assert.NotEqual(settingsLocale.AllEntries, settingsLocale.ExportableEntries);
        IList<IDictionaryEntryError> errors = [];
        Dictionary<string, int> indexCounts = [];
        IEnumerable<KeyValuePair<string, string>> readEntries = settingsLocale.ReadEntries(errors, indexCounts);
        Assert.Empty(errors);
        Assert.Empty(indexCounts);
        Assert.NotEmpty(readEntries);
        Assert.Equal(readEntries, settingsLocale.AllEntries);
    }
}
