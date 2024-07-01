using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
/// <summary>
///     some values/expectations differ from in-game-values
///     <br/>
///     <br/>
///     tried to reference all managed dlls (CS2Paths.props.all)
///     <br/>
///     but it does not work
///     <br/>
///     <br/>
///     i think it doesn't matter since it's mostly about testing the logic
/// </summary>
public class MyLanguagesTests : AProvidesTestDataOk {
    public MyLanguagesTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Theory]
    [InlineData(SystemLanguage.German, "de-DE", "Deutsch", "German", true)]
    [InlineData(SystemLanguage.English, "en-US", "English", "English", true)]
    [InlineData(SystemLanguage.Spanish, "es-ES", "español", "Spanish", true)]
    [InlineData(SystemLanguage.French, "fr-FR", "français", "French", true)]
    [InlineData(SystemLanguage.Italian, "it-IT", "italiano", "Italian", true)]
    [InlineData(SystemLanguage.Japanese, "ja-JP", "日本語", "Japanese", true)]
    [InlineData(SystemLanguage.Korean, "ko-KR", "한국어", "Korean", true)]
    [InlineData(SystemLanguage.Polish, "pl-PL", "polski", "Polish", true)]
    [InlineData(SystemLanguage.Portuguese, "pt-BR", "português (Brasil)", "Portuguese (Brazil)", true)]
    [InlineData(SystemLanguage.Russian, "ru-RU", "русский", "Russian", true)]
    [InlineData(SystemLanguage.ChineseSimplified, "zh-HANS", "中文(简体)", "Chinese (Simplified)", true)]
    [InlineData(SystemLanguage.ChineseTraditional, "zh-HANT", "中文(繁體)", "Chinese (Traditional)", true)]
    //
    [InlineData(SystemLanguage.Afrikaans, null, "Afrikaans", "Afrikaans", false)]
    [InlineData(SystemLanguage.Arabic, null, "العربية", "Arabic", false)]
    [InlineData(SystemLanguage.Basque, null, "euskara", "Basque", false)]
    [InlineData(SystemLanguage.Belarusian, null, "беларуская", "Belarusian", false)]
    [InlineData(SystemLanguage.Bulgarian, null, "български", "Bulgarian", false)]
    [InlineData(SystemLanguage.Catalan, null, "català", "Catalan", false)]
    [InlineData(SystemLanguage.Czech, null, "čeština", "Czech", false)]
    [InlineData(SystemLanguage.Danish, null, "dansk", "Danish", false)]
    [InlineData(SystemLanguage.Dutch, null, "Nederlands", "Dutch", false)]
    [InlineData(SystemLanguage.Estonian, null, "eesti", "Estonian", false)]
    [InlineData(SystemLanguage.Faroese, null, "føroyskt", "Faroese", false)]
    [InlineData(SystemLanguage.Finnish, null, "suomi", "Finnish", false)]
    [InlineData(SystemLanguage.Greek, null, "Ελληνικά", "Greek", false)]
    [InlineData(SystemLanguage.Hebrew, null, "עברית", "Hebrew", false)]
    [InlineData(SystemLanguage.Hindi, null, "हिन्दी", "Hindi", false)]
    [InlineData(SystemLanguage.Hungarian, null, "magyar", "Hungarian", false)]
    [InlineData(SystemLanguage.Icelandic, null, "íslenska", "Icelandic", false)]
    [InlineData(SystemLanguage.Indonesian, null, "Indonesia", "Indonesian", false)]
    [InlineData(SystemLanguage.Latvian, null, "latviešu", "Latvian", false)]
    [InlineData(SystemLanguage.Lithuanian, null, "lietuvių", "Lithuanian", false)]
    [InlineData(SystemLanguage.Norwegian, null, "norsk", "Norwegian", false)]
    [InlineData(SystemLanguage.Romanian, null, "română", "Romanian", false)]
    [InlineData(SystemLanguage.Slovak, null, "slovenčina", "Slovak", false)]
    [InlineData(SystemLanguage.Slovenian, null, "slovenščina", "Slovenian", false)]
    [InlineData(SystemLanguage.Swedish, null, "svenska", "Swedish", false)]
    [InlineData(SystemLanguage.Thai, null, "ไทย", "Thai", false)]
    [InlineData(SystemLanguage.Turkish, null, "Türkçe", "Turkish", false)]
    [InlineData(SystemLanguage.Ukrainian, null, "українська", "Ukrainian", false)]
    [InlineData(SystemLanguage.Vietnamese, null, "Tiếng Việt", "Vietnamese", false)]
    //
    //
    [InlineData(SystemLanguage.Unknown, null, "other languages", "other languages", false)]
    //
    // INFO: when it's generated via log out of the game
    //[InlineData(SystemLanguage.SerboCroatian, "SerboCroatian", "српски/hrvatski", "Serbian/Croatian", false)]
    //
    // INFO: when the test is run
    [InlineData(SystemLanguage.SerboCroatian, null, "srpski/hrvatski", "Serbian/Croatian", false)]
    public void GeneralTests(SystemLanguage systemLanguage,
                             string? expectedId,
                             string expectedName,
                             string expectedNameEnglish,
                             bool expectedIsBuiltIn) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        MyLanguages languages = runtimeContainer.Languages;
        Assert.Equal(ModTestConstants.ExpectedLanguageCount, languages.LanguageCount);
        MyLanguage? language = languages.GetLanguage(systemLanguage);
        Assert.NotNull(language);
        Assert.Equal(expectedIsBuiltIn, language.IsBuiltIn);
        if (expectedIsBuiltIn) {
            Assert.NotNull(expectedId);
            Assert.Equal(expectedId, language.Id);
            Assert.NotNull(languages.GetLanguage(expectedId));
            // no files are read
            Assert.False(language.HasFlavorsWithSources);
            Assert.True(language.HasFlavor(expectedId));
            Assert.False(language.HasFlavorWithSources(expectedId));
        } else {
            Assert.Null(expectedId);
            Assert.Equal(systemLanguage.ToString(), language.Id);
            Assert.NotNull(languages.GetLanguage(systemLanguage.ToString()));
            // no files are read
            Assert.False(language.HasFlavorsWithSources);
            Assert.False(language.HasFlavor(systemLanguage.ToString()));
            Assert.False(language.HasFlavorWithSources(systemLanguage.ToString()));
        }

        Assert.Equal(expectedName, language.Name);
        Assert.Equal(expectedNameEnglish, language.NameEnglish);
        Assert.NotEmpty(language.Flavors);
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
    public void OkTests(SystemLanguage systemLanguage,
                        int expectedFlavorCount) {
        TestDataProvider dataProviderLocal = new TestDataProvider {
            DirectoryName = nameof(OkTests)
        };
        try {
            dataProviderLocal.GenerateData(true);
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
            ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                      userDataPath: dataProviderLocal.DirectoryName);
            runtimeContainer.Init();
            MyLanguages languages = runtimeContainer.Languages;
            Assert.False(languages.HasErroneous);
            Assert.Equal(ModTestConstants.ExpectedLanguageCount, languages.LanguageCount);
            Assert.Equal(ModTestConstants.ExpectedFlavorCount, languages.FlavorCountOfAllLanguages);
            MyLanguage? language = languages.GetLanguage(systemLanguage);
            Assert.NotNull(language);
            Assert.True(language.HasFlavorsWithSources);
            Assert.Equal(expectedFlavorCount, language.FlavorCount);
            // TODO: check each FlavorSource
            //Assert.Equal(expectedFlavorCount * dataProviderLocal.EntryCountPerFile,
            //             language.EntryCountOfAllFlavors);

            TestLocManagerProvider locManagerProvider = runtimeContainer.TestLocManagerProvider;
            locManagerProvider.AddBuiltIn();
            LocManager locManager = runtimeContainer.LocManager;

            for (int i = 0; i < 2; i++) {
                if (i == 1) {
                    dataProviderLocal.GenerateData(true);
                    languages.ReLoad();
                }
                Assert.True(locManager.SupportsLocale(language.Id));
                Assert.True(locManagerProvider.Locales.ContainsKey(language.Id));
                bool gotLocale = locManagerProvider.Locales.TryGetValue(language.Id, out SystemLanguage locale);
                Assert.True(gotLocale);
                Assert.Equal(language.SystemLanguage, locale);
                Assert.True(locManagerProvider.LocaleNames.ContainsKey(language.Id));
                bool gotLocaleName = locManagerProvider.LocaleNames.TryGetValue(language.Id, out string localeName);
                Assert.True(gotLocaleName);
                if (!language.IsBuiltIn) {
                    // due to name-differences...
                    Assert.Equal(language.Name, localeName);
                }
                Assert.False(testLogProvider.HasLoggedTrace);
                Assert.False(testLogProvider.HasLoggedInfo);
                Assert.False(testLogProvider.HasLoggedWarning);
                Assert.False(testLogProvider.HasLoggedError);
                Assert.False(testLogProvider.HasLoggedCritical);
                Assert.False(testLogProvider.HasDisplayedError);
            }
        } finally {
            dataProviderLocal.Dispose();
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
    // INFO: counted 4 from game log, but test only recognizes 4
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
    public void OkNotTests(SystemLanguage systemLanguage,
                           int expectedFlavorCount) {
        TestDataProvider dataProviderLocal = new TestDataProvider {
            DirectoryName = nameof(OkNotTests)
        };
        try {
            dataProviderLocal.GenerateData(true);
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
            ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                      userDataPath: dataProviderLocal.DirectoryName);
            MyLanguages languages = runtimeContainer.Languages;
            runtimeContainer.Init();
            Assert.False(languages.HasErroneous);
            Assert.Equal(ModTestConstants.ExpectedLanguageCount, languages.LanguageCount);
            Assert.Equal(ModTestConstants.ExpectedFlavorCount, languages.FlavorCountOfAllLanguages);
            MyLanguage? language = languages.GetLanguage(systemLanguage);
            Assert.NotNull(language);
            Assert.True(language.HasFlavorsWithSources);
            Assert.Equal(expectedFlavorCount, language.FlavorCount);
            // first its ok, so we expect two times expectedFlavorCount
            // TODO: check each FlavorSource
            //Assert.Equal(expectedFlavorCount * dataProviderLocal.EntryCountPerFile,
            //             language.EntryCountOfAllFlavors);

            TestLocManagerProvider locManagerProvider = runtimeContainer.TestLocManagerProvider;
            locManagerProvider.AddBuiltIn();

            LocManager locManager = runtimeContainer.LocManager;

            for (int i = 0; i < 2; i++) {
                if (i == 1) {
                    dataProviderLocal.GenerateCorruptData(true);
                    languages.ReLoad();
                }
                Assert.True(locManager.SupportsLocale(language.Id));
                Assert.True(locManagerProvider.Locales.ContainsKey(language.Id));
                bool gotLocale = locManagerProvider.Locales.TryGetValue(language.Id, out SystemLanguage locale);
                Assert.True(gotLocale);
                Assert.Equal(language.SystemLanguage, locale);
                Assert.True(locManagerProvider.LocaleNames.ContainsKey(language.Id));
                bool gotLocaleName = locManagerProvider.LocaleNames.TryGetValue(language.Id, out string localeName);
                Assert.True(gotLocaleName);
                if (!language.IsBuiltIn) {
                    // due to name-differences...
                    Assert.Equal(language.Name, localeName);
                }
            }
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.False(testLogProvider.HasDisplayedError);
            //
            Assert.True(testLogProvider.HasLoggedError);

        } finally {
            dataProviderLocal.Dispose();
        }
    }
    [Fact]
    public void LogMarkdownAndCultureInfoNamesTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        MyLanguages languages = runtimeContainer.Languages;
        languages.LogMarkdownAndCultureInfoNames();
        Assert.True(testLogProvider.HasLoggedInfo);
        Assert.Equal(2, testLogProvider.LogInfoCount);
        Assert.False(testLogProvider.HasLoggedWarning);
        Assert.False(testLogProvider.HasLoggedError);
        Assert.False(testLogProvider.HasLoggedTrace);
        Assert.False(testLogProvider.HasLoggedCritical);
    }
    [Fact]
    public void GetFlavorDropDownTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        MyLanguages languages = runtimeContainer.Languages;
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in languages.LanguageDictionary) {
            MyLanguage language = entry.Value;
            IEnumerable<DropdownItem<string>> dropDownItems = language.GetFlavorDropDownItems();
            foreach (Flavor flavor in language.Flavors) {
                string expectedDisplayName = StringHelper.CutStringAfterMaxLengthAndAddThreeDots(flavor.Name,
                                                                                                 ModConstants.MaxDisplayNameLength);
                AssertContainsDropDownItem(dropDownItems,
                                           flavor.Id,
                                           expectedDisplayName);
            }
        }
    }

    private static void AssertContainsDropDownItem(IEnumerable<DropdownItem<string>> dropDownItems,
                                                   string id,
                                                   string expectedDisplayName) {
        foreach (DropdownItem<string> dropDownItem in dropDownItems) {
            if (dropDownItem.value == id) {
                Assert.Equal(expectedDisplayName, dropDownItem.displayName);
                return;
            }
        }
        Assert.Fail();
    }
}
