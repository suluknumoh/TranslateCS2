using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Helpers;
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
        TestDataProvider dataProviderLocal = new TestDataProvider {
            DirectoryName = nameof(ReloadLanguagesOkTest)
        };
        try {
            dataProviderLocal.GenerateData(true);
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
            ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                      userDataPath: dataProviderLocal.DirectoryName);
            runtimeContainer.Init();
            TestLocManagerProvider locManagerProvider = runtimeContainer.TestLocManagerProvider;
            locManagerProvider.AddBuiltIn();
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.False(testLogProvider.HasLoggedError);
            Assert.False(testLogProvider.HasDisplayedError);

            MyLanguages languages = runtimeContainer.Languages;
            ModSettings settings = runtimeContainer.Settings;

            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> oldEntries = [];

            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
                SystemLanguage systemLanguage = entry.Key;
                MyLanguage language = entry.Value;
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    settings.SetFlavor(systemLanguage, flavor.Id);
                    bool gotSettedFlavor = locManagerProvider.Sources.TryGetValue(language.Id, out IList<IDictionarySource> settedFlavorSources);
                    Assert.True(gotSettedFlavor);
                    Assert.NotEmpty(settedFlavorSources);
                    IDictionarySource settedFlavorSource;
                    if (language.IsBuiltIn) {
                        // locManagerProvider adds built in, so two are expected
                        Assert.Equal(2, settedFlavorSources.Count);
                        settedFlavorSource = settedFlavorSources.Last();
                    } else {
                        settedFlavorSource = Assert.Single(settedFlavorSources);
                    }
                    Assert.Equal(flavor, settedFlavorSource);

                    // read entries and copy them and hold them within oldEntries
                    IEnumerable<KeyValuePair<string, string>> readEntries = settedFlavorSource.ReadEntries([], []);
                    List<KeyValuePair<string, string>> readEntriesCopy = [];
                    foreach (KeyValuePair<string, string> readEntry in readEntries) {
                        readEntriesCopy.Add(new KeyValuePair<string, string>(readEntry.Key, readEntry.Value));
                    }
                    oldEntries[flavor.Id] = readEntriesCopy;
                }
            }


            // the testdataprovider now generates a different testdata
            dataProviderLocal.GenerateData(true);
            // that gets reloaded
            settings.ReloadLanguages = true;



            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.False(testLogProvider.HasLoggedError);
            Assert.False(testLogProvider.HasDisplayedError);


            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
                SystemLanguage systemLanguage = entry.Key;
                MyLanguage language = entry.Value;
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    settings.SetFlavor(systemLanguage, flavor.Id);
                    bool gotSettedFlavor = locManagerProvider.Sources.TryGetValue(language.Id, out IList<IDictionarySource> settedFlavorSources);
                    Assert.True(gotSettedFlavor);
                    Assert.NotEmpty(settedFlavorSources);
                    IDictionarySource settedFlavorSource;
                    if (language.IsBuiltIn) {
                        // locManagerProvider adds built in, so two are expected
                        Assert.Equal(2, settedFlavorSources.Count);
                        settedFlavorSource = settedFlavorSources.Last();
                    } else {
                        settedFlavorSource = Assert.Single(settedFlavorSources);
                    }
                    Assert.Equal(flavor, settedFlavorSource);
                    IEnumerable<KeyValuePair<string, string>> newEntries = settedFlavorSource.ReadEntries([], []);
                    // they should be unequal, cause testdataprovider generated different data before reloading
                    Assert.NotEqual(oldEntries[flavor.Id], newEntries);
                }
            }

        } finally {
            dataProviderLocal.Dispose();
        }
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
            TestLocManagerProvider locManagerProvider = runtimeContainer.TestLocManagerProvider;
            locManagerProvider.AddBuiltIn();
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.False(testLogProvider.HasLoggedError);
            Assert.False(testLogProvider.HasDisplayedError);

            MyLanguages languages = runtimeContainer.Languages;
            ModSettings settings = runtimeContainer.Settings;

            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> oldEntries = [];

            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
                SystemLanguage systemLanguage = entry.Key;
                MyLanguage language = entry.Value;
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    settings.SetFlavor(systemLanguage, flavor.Id);
                    bool gotSettedFlavor = locManagerProvider.Sources.TryGetValue(language.Id, out IList<IDictionarySource> settedFlavorSources);
                    Assert.True(gotSettedFlavor);
                    Assert.NotEmpty(settedFlavorSources);
                    IDictionarySource settedFlavorSource;
                    if (language.IsBuiltIn) {
                        // locManagerProvider adds built in, so two are expected
                        Assert.Equal(2, settedFlavorSources.Count);
                        settedFlavorSource = settedFlavorSources.Last();
                    } else {
                        settedFlavorSource = Assert.Single(settedFlavorSources);
                    }
                    Assert.Equal(flavor, settedFlavorSource);

                    // read entries and copy them and hold them within oldEntries
                    IEnumerable<KeyValuePair<string, string>> readEntries = settedFlavorSource.ReadEntries([], []);
                    List<KeyValuePair<string, string>> readEntriesCopy = [];
                    foreach (KeyValuePair<string, string> readEntry in readEntries) {
                        readEntriesCopy.Add(new KeyValuePair<string, string>(readEntry.Key, readEntry.Value));
                    }
                    oldEntries[flavor.Id] = readEntriesCopy;
                }
            }


            // the testdataprovider now generates a different testdata that is also corrupt
            dataProviderLocal.GenerateCorruptData(true);
            // that gets reloaded
            settings.ReloadLanguages = true;



            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.True(testLogProvider.HasLoggedError);
            Assert.True(testLogProvider.HasDisplayedError);


            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
                SystemLanguage systemLanguage = entry.Key;
                MyLanguage language = entry.Value;
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    settings.SetFlavor(systemLanguage, flavor.Id);
                    bool gotSettedFlavor = locManagerProvider.Sources.TryGetValue(language.Id, out IList<IDictionarySource> settedFlavorSources);
                    Assert.True(gotSettedFlavor);
                    Assert.NotEmpty(settedFlavorSources);
                    IDictionarySource settedFlavorSource;
                    if (language.IsBuiltIn) {
                        // locManagerProvider adds built in, so two are expected
                        Assert.Equal(2, settedFlavorSources.Count);
                        settedFlavorSource = settedFlavorSources.Last();
                    } else {
                        settedFlavorSource = Assert.Single(settedFlavorSources);
                    }
                    Assert.Equal(flavor, settedFlavorSource);
                    IEnumerable<KeyValuePair<string, string>> newEntries = settedFlavorSource.ReadEntries([], []);
                    // they should be equal,
                    // cause testdataprovider generated corrupt data before reloading
                    // so the previously existing sources should still be available und used
                    Assert.Equal(oldEntries[flavor.Id], newEntries);
                }
            }

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
        TestLocManagerProvider locManagerProvider = runtimeContainer.TestLocManagerProvider;
        // INFO: TestLocManager has to be manipulated, cause built-in-languages are loaded by the game itself and not by this mod...
        locManagerProvider.AddBuiltIn();

        Dictionary<SystemLanguage, MyLanguage> languages = runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> language in languages) {
            modSettings.FlavorsSetted.Clear();
            IntSettings intSettings = runtimeContainer.IntSettings;
            intSettings.CurrentLocale = language.Value.Id;
            modSettings.Locale = null;
            modSettings.HandleLocaleOnLoad();
            KeyValuePair<SystemLanguage, string> setted = Assert.Single(modSettings.FlavorsSetted);
            Assert.Equal(language.Key, setted.Key);
            if (language.Value.IsBuiltIn) {
                Assert.Equal(DropDownItemsHelper.None, setted.Value);
            } else {
                Assert.Equal(language.Value.Flavors.First().Id, setted.Value);
            }
            Assert.Single(locManagerProvider.Sources[language.Value.Id]);
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
        IntSettings intSettings = runtimeContainer.IntSettings;

        foreach (KeyValuePair<SystemLanguage, MyLanguage> language in runtimeContainer.Languages.LanguageDictionary) {
            modSettings.PreviousLocale = previousLocale;
            modSettings.Locale = language.Value.Id;
            runtimeContainer.Dispose();
            if (language.Value.IsBuiltIn) {
                Assert.Equal(language.Value.Id, intSettings.CurrentLocale);
            } else {
                Assert.Equal(expectedLocale, intSettings.CurrentLocale);
            }
        }
    }
}
