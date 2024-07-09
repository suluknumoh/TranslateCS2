using System;
using System.Collections.Generic;
using System.Linq;

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
    public void SectionNameTest() {
        Assert.Equal("Main", ModSettings.Section);
    }

    [Fact]
    public void SetDefaultsTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.Equal(StringConstants.All, modSettings.ExportDropDown);
        string expectedDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        Assert.Equal(expectedDirectory, modSettings.DefaultDirectory);
        Assert.Equal(expectedDirectory, modSettings.GenerateDirectory);
        Assert.Equal(expectedDirectory, modSettings.ExportDirectory);
        Assert.True(modSettings.LoadFromOtherMods);
    }

    [Fact]
    public void ApplyTest() {
        // Apply(Setting setting);
        // cannot be tested
    }

    [Fact]
    public void ApplyNewLocaleTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        runtimeContainer.TestLocManagerProvider.AddBuiltIn();
        ModSettings modSettings = runtimeContainer.Settings;
        Dictionary<SystemLanguage, MyLanguage> languageDictionary = runtimeContainer.Languages.LanguageDictionary;
        string expectedPreviousBuiltIn = runtimeContainer.LocManager.FallbackLocaleId;
        int expectedValueVersion = 0;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languageDictionary) {
            SystemLanguage systemLanguage = entry.Key;
            MyLanguage language = entry.Value;
            modSettings.ApplyNewLocale(language.Id);
            Assert.Equal(language.Name, modSettings.CurrentLanguage);
            string settedFlavor = modSettings.FlavorsSetted[systemLanguage];
            if (language.IsBuiltIn) {
                Assert.Equal(DropDownItemsHelper.None, settedFlavor);
                expectedPreviousBuiltIn = language.Id;
            } else {
                Assert.NotEqual(DropDownItemsHelper.None, settedFlavor);
            }
            Assert.Equal(expectedPreviousBuiltIn, modSettings.PreviousLocale);
            expectedValueVersion++;
            Assert.Equal(expectedValueVersion, modSettings.GetValueVersion());

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
        Assert.True(modSettings.LoadFromOtherMods);
        Assert.Equal(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), modSettings.DefaultDirectory);
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
}
