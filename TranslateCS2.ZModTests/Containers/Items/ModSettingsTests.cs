using System.Collections.Generic;
using System.IO;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZModTestLib.TestHelpers.Containers;
using TranslateCS2.ZZZModTestLib.TestHelpers.Containers.Items.Unitys;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsTests : AProvidesTestDataOk {
    public ModSettingsTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void GenerateLocalizationJsonTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.False(modSettings.IsGenerateLocalizationJsonHiddenDisabled());
        modSettings.GenerateLocalizationJson = true;
        DirectoryInfo directoryInfo = new DirectoryInfo(runtimeContainer.Paths.ModsDataPathSpecific);
        Assert.True(directoryInfo.Exists);
        IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles(ModConstants.ModExportKeyValueJsonName);
        Assert.NotEmpty(files);
        FileInfo file = Assert.Single(files);
        Assert.True(file.Exists);
        Assert.False(testLogProvider.HasLoggedTrace);
        Assert.False(testLogProvider.HasLoggedInfo);
        Assert.False(testLogProvider.HasLoggedWarning);
        Assert.False(testLogProvider.HasLoggedError);
        Assert.False(testLogProvider.HasLoggedCritical);
    }
    [Fact]
    public void LogMarkdownAndCultureInfoNamesTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        ModSettings modSettings = runtimeContainer.Settings;
        modSettings.LogMarkdownAndCultureInfoNames = true;
        Assert.True(testLogProvider.HasLoggedInfo);
        Assert.Equal(2, testLogProvider.LogInfoCount);
        Assert.False(testLogProvider.HasLoggedWarning);
        Assert.False(testLogProvider.HasLoggedError);
        Assert.False(testLogProvider.HasLoggedTrace);
        Assert.False(testLogProvider.HasLoggedCritical);
    }
    [Fact]
    public void ReloadLanguagesOkTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        Assert.False(testLogProvider.HasLoggedTrace);
        Assert.False(testLogProvider.HasLoggedInfo);
        Assert.False(testLogProvider.HasLoggedWarning);
        Assert.False(testLogProvider.HasLoggedError);
        Assert.False(testLogProvider.HasLoggedCritical);
        Assert.False(testLogProvider.HasDisplayedError);
        ModSettings modSettings = runtimeContainer.Settings;
        modSettings.ReloadLanguages = true;
        Assert.False(testLogProvider.HasLoggedTrace);
        Assert.False(testLogProvider.HasLoggedInfo);
        Assert.False(testLogProvider.HasLoggedWarning);
        Assert.False(testLogProvider.HasLoggedError);
        Assert.False(testLogProvider.HasLoggedCritical);
        Assert.False(testLogProvider.HasDisplayedError);
    }
    [Fact]
    public void ReloadLanguagesOkNotTest() {
        TestDataProvider dataProviderLocal = new TestDataProvider {
            DirectoryName = nameof(ReloadLanguagesOkNotTest)
        };
        try {
            dataProviderLocal.GenerateData(true);
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
            ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                      userDataPath: dataProviderLocal.DirectoryName);
            runtimeContainer.Init();
            ModSettings modSettings = runtimeContainer.Settings;
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedError);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.False(testLogProvider.HasDisplayedError);
            dataProviderLocal.GenerateCorruptData(true);
            modSettings.ReloadLanguages = true;
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.True(testLogProvider.HasLoggedError);
            Assert.True(testLogProvider.HasDisplayedError);
        } finally {
            dataProviderLocal.Dispose();
        }
    }
    [Fact]
    public void HandleLocaleOnLoadTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        TestLocManager locManager = runtimeContainer.TestLocManager;
        // INFO: TestLocManager has to be manipulated, cause built-in-languages are loaded by the game itself and not by this mod...

        // TODO:
    }
    [Fact]
    public void HandleLocaleOnUnLoadTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        TestLocManager locManager = runtimeContainer.TestLocManager;
        // INFO: TestLocManager has to be manipulated, cause built-in-languages are loaded by the game itself and not by this mod...

        // TODO:
    }
}
