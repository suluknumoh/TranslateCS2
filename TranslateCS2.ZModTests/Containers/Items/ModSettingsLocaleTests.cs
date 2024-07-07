using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsLocaleTests : AProvidesTestDataOk {
    public ModSettingsLocaleTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void GeneralTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsLocaleTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        ModSettingsLocale settingsLocale = modSettings.SettingsLocale;
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
