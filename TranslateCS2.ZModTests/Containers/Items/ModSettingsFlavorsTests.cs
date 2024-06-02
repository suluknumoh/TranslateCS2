using System;
using System.Collections.Generic;
using System.Linq;

using Game.UI.Menu;
using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.ZModTests.TestHelpers;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsFlavorsTests : AProvidesTestDataOk {
    public ModSettingsFlavorsTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void AddFlavorsToPageDataTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        AutomaticSettings.SettingPageData pageData = new AutomaticSettings.SettingPageData("a", true);
        modSettings.AddFlavorsToPageData(pageData);
        Assert.NotNull(pageData);
        AutomaticSettings.SettingTabData tab = Assert.Single(pageData.tabs);
        IEnumerable<AutomaticSettings.SettingItemData> items = tab.items;
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        Assert.Equal(ModTestConstants.ExpectedLanguageCount, items.Count());
        foreach (AutomaticSettings.SettingItemData item in items) {
            MyFlavorDropDownSettingItemData myItem = Assert.IsType<MyFlavorDropDownSettingItemData>(item);
            Assert.Equal(AutomaticSettings.WidgetType.StringDropdown, myItem.widgetType);
            Assert.Equal(ModSettings.FlavorGroup, myItem.simpleGroup);
            Assert.True(myItem.IsHidden());
            Assert.True(myItem.IsDisabled());
            DropdownField<string> dropDown = Assert.IsType<DropdownField<string>>(myItem.widget);
            Assert.NotNull(dropDown);
            MyLanguage language = myItem.Language;
            Assert.NotNull(language);
            // to make items hideAction return false;
            // its checked within the update method
            runtimeContainer.IntSettings.CurrentLocale = language.Id;
            //WidgetChanges change = dropDown.Update();
            myItem.UpdateWidget();
            // after the dropdown is updated,
            // it has DropDownItem's we can check againts languages flavorcount
            int expectedDropDownItemCount = language.FlavorCount;
            if (language.IsBuiltIn) {
                // builtins also should have an extra entry 'none'
                expectedDropDownItemCount++;
            }
            Assert.Equal(expectedDropDownItemCount, dropDown.items.Length);
        }
    }
    [Fact]
    public void FlavorsSettedSetterGetterTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        Dictionary<SystemLanguage, MyLanguage> languageDictionary = runtimeContainer.Languages.LanguageDictionary;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languageDictionary) {
            SystemLanguage systemLanguage = entry.Key;
            MyLanguage language = entry.Value;
            foreach (TranslationFile translation in language.Flavors) {
                modSettings.SetFlavor(systemLanguage, translation.Id);
                Assert.Contains(systemLanguage, modSettings.FlavorsSetted.Keys);
                modSettings.FlavorsSetted.TryGetValue(systemLanguage, out string result);
                Assert.Equal(translation.Id, result);
                string getterResult = modSettings.GetSettedFlavor(systemLanguage);
                Assert.Equal(translation.Id, getterResult);
            }
        }
        Assert.Equal(ModTestConstants.ExpectedLanguageCount, modSettings.FlavorsSetted.Count);
    }
    [Fact]
    public void FlavorsSettedTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        Dictionary<SystemLanguage, MyLanguage> languageDictionary = runtimeContainer.Languages.LanguageDictionary;
        Dictionary<SystemLanguage, string> testData = [];
        foreach (SystemLanguage entry in languageDictionary.Keys) {
            // AAA: entry.ToString() to test the Getter with an incorrect value
            testData.Add(entry, entry.ToString());
        }
        modSettings.FlavorsSetted = null;
        Assert.NotNull(modSettings.FlavorsSetted);
        modSettings.FlavorsSetted.Clear();
        Assert.Empty(modSettings.FlavorsSetted);
        modSettings.FlavorsSetted = testData;
        Assert.NotNull(modSettings.FlavorsSetted);
        Assert.NotEmpty(modSettings.FlavorsSetted);
        Assert.Equal(testData, modSettings.FlavorsSetted);
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languageDictionary) {
            string result = modSettings.GetSettedFlavor(entry.Key);
            if (entry.Value.IsBuiltIn) {
                // see: AAA
                Assert.Equal(DropDownItems.None, result);
            } else {
                Assert.Equal(entry.Value.Flavors.First().Id, result);
            }
        }
    }
    [Fact]
    public void FlavorsSettedSetterGetterWithChineseTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        SystemLanguage systemLanguage = SystemLanguage.Chinese;
        modSettings.SetFlavor(systemLanguage, systemLanguage.ToString());
        Assert.Contains(systemLanguage, modSettings.FlavorsSetted.Keys);
        modSettings.FlavorsSetted.TryGetValue(systemLanguage, out string result);
        Assert.Equal(DropDownItems.None, result);
        string getterResult = modSettings.GetSettedFlavor(systemLanguage);
        Assert.Equal(DropDownItems.None, getterResult);
    }
}
