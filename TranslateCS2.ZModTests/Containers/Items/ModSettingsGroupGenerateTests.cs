using System;
using System.Collections.Generic;
using System.IO;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsGroupGenerateTests : AProvidesTestDataOk {
    private readonly string tempPath = Path.GetTempPath();
    public ModSettingsGroupGenerateTests(TestDataProvider testDataProvider) : base(testDataProvider) { }

    [Fact]
    public void GroupNameTest() {
        Assert.Equal("GenerateGroup", ModSettings.GenerateGroup);
    }

    [Fact]
    public void GenerateDirectoryTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupGenerateTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // no need to init the container for this test
        //runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.Equal(modSettings.DefaultDirectory, modSettings.GenerateDirectory);
    }

    [Fact]
    public void LogMarkdownAndCultureInfoNamesTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupGenerateTests>();
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
    public void GenerateLocalizationJsonTest() {
        FileInfo? generated = null;
        try {
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupGenerateTests>();
            ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                      userDataPath: this.dataProvider.DirectoryName);
            runtimeContainer.Init();
            ModSettings modSettings = runtimeContainer.Settings;
            Assert.True(modSettings.LoadFromOtherMods);
            Assert.Equal(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), modSettings.DefaultDirectory);

            modSettings.GenerateDirectory = this.tempPath;
            Assert.Equal(this.tempPath, modSettings.GenerateDirectory);


            modSettings.GenerateLocalizationJson = true;
            DirectoryInfo directoryInfo = new DirectoryInfo(modSettings.GenerateDirectory);
            Assert.True(directoryInfo.Exists);
            IEnumerable<FileInfo> files = directoryInfo.EnumerateFiles(ModConstants.ModExportKeyValueJsonName);
            Assert.NotEmpty(files);
            generated = Assert.Single(files);
            Assert.True(generated.Exists);
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedError);
            Assert.False(testLogProvider.HasLoggedCritical);
        } finally {
            generated?.Delete();
        }
    }
}
