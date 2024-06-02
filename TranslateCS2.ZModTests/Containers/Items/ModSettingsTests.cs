using System.Collections.Generic;
using System.IO;
using System.Linq;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsTests : AProvidesTestDataOk {
    public ModSettingsTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void GenerateLocalizationJsonTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
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
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
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
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
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
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
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
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // only init languages, runtimeContainers init does to much for this test method
        runtimeContainer.Languages.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        TestLocManagerProvider locManager = runtimeContainer.TestLocManagerProvider;
        // INFO: TestLocManager has to be manipulated, cause built-in-languages are loaded by the game itself and not by this mod...
        locManager.AddBuiltIn();

        Dictionary<SystemLanguage, MyLanguage> languages = runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> language in languages) {
            modSettings.FlavorsSetted.Clear();
            IIntSettings intSettings = runtimeContainer.IntSettings;
            intSettings.CurrentLocale = language.Value.Id;
            modSettings.Locale = null;
            modSettings.HandleLocaleOnLoad();
            KeyValuePair<SystemLanguage, string> setted = Assert.Single(modSettings.FlavorsSetted);
            Assert.Equal(language.Key, setted.Key);
            if (language.Value.IsBuiltIn) {
                Assert.Equal(DropDownItems.None, setted.Value);
            } else {
                Assert.Equal(language.Value.Flavors.First().Id, setted.Value);
            }
            Assert.Single(locManager.Sources[language.Value.Id]);
        }
    }
    [Theory]
    [InlineData(null, "os")]
    [InlineData("de-DE", "de-DE")]
    [InlineData("en-US", "en-US")]
    [InlineData("es-ES", "es-ES")]
    [InlineData("fr-FR", "fr-FR")]
    [InlineData("it-IT", "it-IT")]
    [InlineData("ja-JP", "ja-JP")]
    [InlineData("ko-KR", "ko-KR")]
    [InlineData("pl-PL", "pl-PL")]
    [InlineData("pt-BR", "pt-BR")]
    [InlineData("ru-RU", "ru-RU")]
    [InlineData("zh-HANS", "zh-HANS")]
    [InlineData("zh-HANT", "zh-HANT")]
    public void HandleLocaleOnUnLoadTest(string? previousLocale, string expectedLocale) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // only init languages, runtimeContainers init does to much for this test method
        runtimeContainer.Languages.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        IIntSettings intSettings = runtimeContainer.IntSettings;

        foreach (KeyValuePair<SystemLanguage, MyLanguage> language in runtimeContainer.Languages.LanguageDictionary) {
            modSettings.PreviousLocale = previousLocale;
            modSettings.Locale = language.Value.Id;
            runtimeContainer.Dispose();
            if (language.Value.IsBuiltIn) {
                Assert.Equal(language.Value.Id, intSettings.CurrentLocale);
                Assert.Equal(language.Value.Id, intSettings.Locale);
            } else {
                Assert.Equal(expectedLocale, intSettings.CurrentLocale);
                Assert.Equal(expectedLocale, intSettings.Locale);
            }
        }
    }
}
