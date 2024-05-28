using System;
using System.Collections.Generic;
using System.Linq;

using Game.UI.Menu;
using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
[Collection("TestDataOK")]
public class ModSettingsFlavorsTests {
    private readonly TestDataProvider dataProvider;
    public ModSettingsFlavorsTests(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
    }
    [Fact]
    public void AddFlavorsToPageDataTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
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
        // due to SystemLanguage.Chinese
        int expectedSize = systemLanguages.Count() - 1;
        Assert.Equal(expectedSize, items.Count());
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
            WidgetChanges change = dropDown.Update();
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
}
