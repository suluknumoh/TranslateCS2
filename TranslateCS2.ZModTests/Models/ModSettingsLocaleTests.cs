using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Models;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

namespace TranslateCS2.ZModTests.Models;
public class ModSettingsLocaleTests {
    [Fact]
    public void GeneralTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsLocaleTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider);
        ModSettings modSettings = new ModSettings(runtimeContainer);
        ModSettingsLocale settingsLocale = new ModSettingsLocale(modSettings, runtimeContainer);
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
