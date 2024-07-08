using System;
using System.Collections.Generic;
using System.Linq;

using Colossal;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsGroupReloadTests : AProvidesTestDataOk {
    public ModSettingsGroupReloadTests(TestDataProvider testDataProvider) : base(testDataProvider) { }

    [Fact]
    public void GroupNameTest() {
        Assert.Equal("ReloadGroup", ModSettings.ReloadGroup);
    }

    [Fact]
    public void ReloadLanguagesOkTest() {
        TestDataProvider dataProviderLocal = new TestDataProvider {
            DirectoryName = nameof(ReloadLanguagesOkTest)
        };
        try {
            dataProviderLocal.GenerateData(true);
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupReloadTests>();
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
            ModSettings modSettings = runtimeContainer.Settings;
            Assert.True(modSettings.LoadFromOtherMods);
            Assert.Equal(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), modSettings.DefaultDirectory);

            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> oldEntries = [];

            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
                SystemLanguage systemLanguage = entry.Key;
                MyLanguage language = entry.Value;
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    modSettings.SetFlavor(systemLanguage, flavor.Id);
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
            modSettings.ReloadLanguages = true;



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
                    modSettings.SetFlavor(systemLanguage, flavor.Id);
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
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupReloadTests>();
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
            ModSettings modSettings = runtimeContainer.Settings;
            Assert.True(modSettings.LoadFromOtherMods);
            Assert.Equal(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), modSettings.DefaultDirectory);

            Dictionary<string, IEnumerable<KeyValuePair<string, string>>> oldEntries = [];

            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
                SystemLanguage systemLanguage = entry.Key;
                MyLanguage language = entry.Value;
                IList<Flavor> flavors = language.Flavors;
                foreach (Flavor flavor in flavors) {
                    modSettings.SetFlavor(systemLanguage, flavor.Id);
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
            modSettings.ReloadLanguages = true;



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
                    modSettings.SetFlavor(systemLanguage, flavor.Id);
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
}
