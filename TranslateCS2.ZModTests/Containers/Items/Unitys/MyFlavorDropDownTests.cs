using System;
using System.Collections.Generic;

using Game.UI.Menu;
using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items.Unitys;
public class MyFlavorDropDownTests : AProvidesTestDataOk {
    public MyFlavorDropDownTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void AddFlavorsToPageDataTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyFlavorDropDownTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;
        runtimeContainer.Init();
        ModSettings modSettings = runtimeContainer.Settings;
        MyLanguages languages = runtimeContainer.Languages;
        foreach (MyLanguage language in languages.LanguageDictionary.Values) {
            MyFlavorDropDownSettingItemData myItem = MyFlavorDropDownSettingItemData.Create(language,
                                                                                            modSettings,
                                                                                            "does_not_matter_in_this_case");
            AutomaticSettings.SettingPageData pageData = new AutomaticSettings.SettingPageData("", false);
            myItem.pageData = pageData;
            Assert.Equal(AutomaticSettings.WidgetType.StringDropdown, myItem.widgetType);
            Assert.Equal(ModSettings.FlavorGroup, myItem.simpleGroup);
            Assert.True(myItem.IsHidden());
            Assert.True(myItem.IsDisabled());
            Assert.NotNull(myItem.widget);
            DropdownField<string> dropDown = Assert.IsType<DropdownField<string>>(myItem.widget);
            Assert.NotNull(dropDown);
            Assert.NotNull(myItem.Language);
            Assert.Equal(language, myItem.Language);
            // to make items hideAction return false;
            // its checked within the update method
            runtimeContainer.IntSettings.CurrentLocale = language.Id;
            //WidgetChanges change = dropDown.Update();
            myItem.UpdateWidget();
            // after the dropdown is updated,
            // it has DropDownItem's we can check againts languages flavorcount
            Assert.False(myItem.IsHidden());
            Assert.False(myItem.IsDisabled());
            int expectedDropDownItemCount = language.FlavorCount;
            if (language.IsBuiltIn) {
                // builtins also should have an extra entry 'none'
                expectedDropDownItemCount++;
            }
            Assert.Equal(expectedDropDownItemCount, dropDown.items.Length);

            modSettings.FlavorsSetted.Clear();
            MyFlavorDropDownSettingProperty property = Assert.IsType<MyFlavorDropDownSettingProperty>(myItem.property);
            foreach (DropdownItem<string>? dropDownItem in dropDown.items) {
                property.setter.Invoke(modSettings, dropDownItem.value);
                KeyValuePair<SystemLanguage, string> flavorSetted = Assert.Single(modSettings.FlavorsSetted);
                Assert.Equal(language.SystemLanguage, flavorSetted.Key);
                Assert.Equal(dropDownItem.value, flavorSetted.Value);
            }

        }
    }
}
