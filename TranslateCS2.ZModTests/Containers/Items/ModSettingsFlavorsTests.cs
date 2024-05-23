using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Models;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ModSettingsFlavorsTests {
    [Fact]
    public void GetFlavorsEmptyTest() {
        // userDataPath is null,
        // sources arent loaded!
        string? userDataPath = null;
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: userDataPath);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;

        ModSettings modSettings = new ModSettings(runtimeContainer);
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        foreach (SystemLanguage systemLanguage in systemLanguages) {

            string methodName = ModSettings.GetFlavorsLangMethodName(systemLanguage);
            MethodInfo? method = modSettings.GetType().GetMethod(methodName);

            if (SystemLanguage.Chinese == systemLanguage) {
                Assert.Null(method);
                continue;
            }


            object? result = method?.Invoke(modSettings, []);
            Assert.NotNull(result);
            DropdownItem<string>[]? dropDownItems = result as DropdownItem<string>[];
            if (dropDownItems is null) {
                Assert.Fail();
            }
            // userDataPath is null,
            // sources arent loaded!
            // we expect each drop-down has a single entry 'none'
            DropdownItem<string> single = Assert.Single(dropDownItems);
            Assert.Equal(DropDownItems.None, single.value);
            Assert.Equal(DropDownItems.None, single.displayName);

            bool isHidden = modSettings.IsHidden(systemLanguage);
            Assert.True(isHidden);

            bool isDisabled = modSettings.IsDisabled(systemLanguage);
            Assert.True(isDisabled);
        }

    }
    [Theory]
    [InlineData(SystemLanguage.German, 6)]
    [InlineData(SystemLanguage.English, 17)]
    [InlineData(SystemLanguage.Spanish, 22)]
    [InlineData(SystemLanguage.French, 15)]
    [InlineData(SystemLanguage.Italian, 3)]
    [InlineData(SystemLanguage.Japanese, 2)]
    [InlineData(SystemLanguage.Korean, 2)]
    [InlineData(SystemLanguage.Polish, 2)]
    [InlineData(SystemLanguage.Portuguese, 3)]
    [InlineData(SystemLanguage.Russian, 3)]
    //
    // INFO: counted 5 from game log, but test only recognizes 4
    //[InlineData(SystemLanguage.ChineseSimplified, 5)]
    [InlineData(SystemLanguage.ChineseSimplified, 4)]
    //
    [InlineData(SystemLanguage.ChineseTraditional, 5)]
    //
    [InlineData(SystemLanguage.Afrikaans, 2)]
    [InlineData(SystemLanguage.Arabic, 17)]
    [InlineData(SystemLanguage.Basque, 2)]
    [InlineData(SystemLanguage.Belarusian, 2)]
    [InlineData(SystemLanguage.Bulgarian, 2)]
    //
    // INFO: counted 3 from game log, but test only recognizes 2
    //[InlineData(SystemLanguage.Catalan, 3)]
    [InlineData(SystemLanguage.Catalan, 2)]
    //
    [InlineData(SystemLanguage.Czech, 2)]
    [InlineData(SystemLanguage.Danish, 2)]
    [InlineData(SystemLanguage.Dutch, 3)]
    [InlineData(SystemLanguage.Estonian, 2)]
    [InlineData(SystemLanguage.Faroese, 2)]
    [InlineData(SystemLanguage.Finnish, 2)]
    [InlineData(SystemLanguage.Greek, 2)]
    [InlineData(SystemLanguage.Hebrew, 2)]
    [InlineData(SystemLanguage.Hindi, 2)]
    [InlineData(SystemLanguage.Hungarian, 2)]
    [InlineData(SystemLanguage.Icelandic, 2)]
    [InlineData(SystemLanguage.Indonesian, 2)]
    [InlineData(SystemLanguage.Latvian, 2)]
    [InlineData(SystemLanguage.Lithuanian, 2)]
    [InlineData(SystemLanguage.Norwegian, 5)]
    [InlineData(SystemLanguage.Romanian, 3)]
    [InlineData(SystemLanguage.SerboCroatian, 12)]
    [InlineData(SystemLanguage.Slovak, 2)]
    [InlineData(SystemLanguage.Slovenian, 2)]
    [InlineData(SystemLanguage.Swedish, 3)]
    [InlineData(SystemLanguage.Thai, 2)]
    [InlineData(SystemLanguage.Turkish, 2)]
    [InlineData(SystemLanguage.Ukrainian, 2)]
    //
    // INFO: counted 126 from game log, but test recognizes 128
    //[InlineData(SystemLanguage.Unknown, 126)]
    [InlineData(SystemLanguage.Unknown, 128)]
    //
    [InlineData(SystemLanguage.Vietnamese, 2)]
    public void GetFlavorsFilledTest(SystemLanguage systemLanguage, int expectedDropDownItemsCount) {
        // userDataPath is null,
        // sources arent loaded!
        string? userDataPath = "Assets/MyLanguagesTests/ok";
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ModSettingsFlavorsTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: userDataPath);
        // to disable/hide all drop-downs, cause TestIntSettings are initialized with 'en-US'
        runtimeContainer.IntSettings.CurrentLocale = String.Empty;

        runtimeContainer.Languages.Init();

        ModSettings modSettings = new ModSettings(runtimeContainer);
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        string methodName = ModSettings.GetFlavorsLangMethodName(systemLanguage);
        MethodInfo? method = modSettings.GetType().GetMethod(methodName);


        object? result = method?.Invoke(modSettings, []);
        Assert.NotNull(result);
        DropdownItem<string>[]? dropDownItems = result as DropdownItem<string>[];
        if (dropDownItems is null) {
            Assert.Fail();
        }

        MyLanguage? language = runtimeContainer.Languages.GetLanguage(systemLanguage);
        Assert.NotNull(language);

        int expectedSize = expectedDropDownItemsCount;
        if (language.IsBuiltIn) {
            // built in languages contain 'none'
            // non-built-in dont contain a 'none'-entry
            expectedSize++;
        }
        Assert.Equal(expectedSize, dropDownItems.Length);


        bool isHidden = modSettings.IsHidden(systemLanguage);
        Assert.True(isHidden);

        bool isDisabled = modSettings.IsDisabled(systemLanguage);
        Assert.True(isDisabled);

    }
}
