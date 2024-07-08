using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsGroupExportTests : AProvidesTestDataOk {
    public ModSettingsGroupExportTests(TestDataProvider testDataProvider) : base(testDataProvider) { }

    [Fact]
    public void GroupNameTest() {
        Assert.Equal("ExportGroup", ModSettings.ExportGroup);
    }

    [Fact]
    public void ExportDropDownTest() {
        // in my holy opinion,
        // it does not make sense to test a property
        // without logic
    }

    [Fact]
    public void GetExportDropDownItemsTest() {
        // testing the respective method
        // would require a mock of IAssetDatabase
        // and a wrapper/container that transports the LocaleAsset, or a mock of it
    }

    [Fact]
    public void ExportDirectoryTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupExportTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // no need to init the container for this test
        //runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.Equal(modSettings.DefaultDirectory, modSettings.ExportDirectory);
    }

    [Fact]
    public void ExportButtonTest() {
        // testing the respective method
        // would require a mock of IAssetDatabase
        // and a wrapper/container that transports the LocaleAsset, or a mock of it
    }
}
