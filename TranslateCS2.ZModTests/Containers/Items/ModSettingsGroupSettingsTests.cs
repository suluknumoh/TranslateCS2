using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsGroupSettingsTests : AProvidesTestDataOk {
    public ModSettingsGroupSettingsTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void GroupNameTest() {
        Assert.Equal("SettingsGroup", ModSettings.SettingsGroup);
    }

    [Fact]
    public void LoadFromOtherModsTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupSettingsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.True(modSettings.LoadFromOtherMods);
    }
}
