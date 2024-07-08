using System;
using System.Collections.Generic;
using System.Linq;

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
