using System;
using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsGroupFlavorsTests : AProvidesTestDataOk {
    public ModSettingsGroupFlavorsTests(TestDataProvider testDataProvider) : base(testDataProvider) { }

    [Fact]
    public void GroupNameTest() {
        Assert.Equal("FlavorGroup", ModSettings.FlavorGroup);
    }

    [Fact]
    public void GetValueVersionTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.Equal(0, modSettings.GetValueVersion());
    }

    [Theory]
    [InlineData(SystemLanguage.Norwegian, "no", 1)]
    [InlineData(null, null, 0)]
    public void FlavorsSettedTest(SystemLanguage? systemLanguage, string? flavorId, int expectedCount) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        ModSettings modSettings = runtimeContainer.Settings;

        Dictionary<SystemLanguage, string>? flavors = null;
        if (systemLanguage is null
            || flavorId is null) {
            modSettings.FlavorsSetted = flavors;
        } else {
            flavors = [];
            flavors.Add((SystemLanguage) systemLanguage, flavorId);
            modSettings.FlavorsSetted = flavors;
        }
        Assert.NotNull(modSettings.FlavorsSetted);
        Assert.Equal(expectedCount, modSettings.FlavorsSetted.Count);
    }

    [Fact]
    public void CurrentLanguageTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        ModSettings modSettings = runtimeContainer.Settings;
        Assert.Equal(String.Empty, modSettings.CurrentLanguage);
    }

    [Fact]
    public void FlavorDropDownTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        runtimeContainer.TestLocManagerProvider.AddBuiltIn();
        ModSettings modSettings = runtimeContainer.Settings;
        IntSettings intSettings = runtimeContainer.IntSettings;
        Dictionary<SystemLanguage, MyLanguage> languages = runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages) {
            SystemLanguage systemLanguage = entry.Key;
            MyLanguage language = entry.Value;
            intSettings.CurrentLocale = language.Id;
            modSettings.Locale = language.Id;
            if (language.IsBuiltIn) {
                Assert.Equal(DropDownItemsHelper.None, modSettings.FlavorDropDown);
            } else {
                Assert.NotEqual(DropDownItemsHelper.None, modSettings.FlavorDropDown);
            }
            foreach (Flavor flavor in language.Flavors) {
                modSettings.FlavorDropDown = flavor.Id;
                Assert.Equal(flavor.Id, modSettings.FlavorDropDown);
            }
        }
    }

    [Fact]
    public void GetFlavorDropDownItemsTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        runtimeContainer.TestLocManagerProvider.AddBuiltIn();
        ModSettings modSettings = runtimeContainer.Settings;
        IntSettings intSettings = runtimeContainer.IntSettings;
        Dictionary<SystemLanguage, MyLanguage> languages = runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages) {
            SystemLanguage systemLanguage = entry.Key;
            MyLanguage language = entry.Value;
            intSettings.CurrentLocale = language.Id;
            modSettings.Locale = language.Id;
            DropdownItem<string>[] dropDownItems = modSettings.GetFlavorDropDownItems();
            int expectedCount = language.FlavorCount;
            if (language.IsBuiltIn) {
                expectedCount++;
            }
            Assert.Equal(expectedCount, dropDownItems.Length);
        }
    }

    [Fact]
    public void IsFlavorDropDownDisabledTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsGroupFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        runtimeContainer.TestLocManagerProvider.AddBuiltIn();
        ModSettings modSettings = runtimeContainer.Settings;
        IntSettings intSettings = runtimeContainer.IntSettings;
        Dictionary<SystemLanguage, MyLanguage> languages = runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages) {
            SystemLanguage systemLanguage = entry.Key;
            MyLanguage language = entry.Value;
            intSettings.CurrentLocale = language.Id;
            Assert.False(modSettings.IsFlavorDropDownDisabled());
        }
    }

    [Fact]
    public void SetFlavorTest() {
        // is implicitly tested
    }

    [Fact]
    public void GetSettedFlavorTest() {
        // is implicitly tested
    }
}
