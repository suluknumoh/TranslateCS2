using System.Collections.Generic;

using Colossal;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZModTestLib.Containers.Items.Unitys;
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
[Collection("TestDataOK")]
public class MyLanguagesTests {
    private readonly TestDataProvider dataProvider;
    public MyLanguagesTests(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
    }
    [Theory]
    [InlineData(SystemLanguage.German, "de-DE", "Deutsch", "German", true)]
    [InlineData(SystemLanguage.English, "en-US", "English", "English", true)]
    [InlineData(SystemLanguage.Spanish, "es-ES", "español", "Spanish", true)]
    [InlineData(SystemLanguage.French, "fr-FR", "français", "French", true)]
    [InlineData(SystemLanguage.Italian, "it-IT", "italiano", "Italian", true)]
    [InlineData(SystemLanguage.Japanese, "ja-JP", "日本語", "Japanese", true)]
    [InlineData(SystemLanguage.Korean, "ko-KR", "한국어", "Korean", true)]
    [InlineData(SystemLanguage.Polish, "pl-PL", "polski", "Polish", true)]
    [InlineData(SystemLanguage.Portuguese, "pt-BR", "português", "Portuguese", true)]
    [InlineData(SystemLanguage.Russian, "ru-RU", "русский", "Russian", true)]
    [InlineData(SystemLanguage.ChineseSimplified, "zh-HANS", "中文(简体)", "Chinese (Simplified)", true)]
    [InlineData(SystemLanguage.ChineseTraditional, "zh-HANT", "中文(繁體)", "Chinese (Traditional)", true)]
    //
    [InlineData(SystemLanguage.Afrikaans, "af", "Afrikaans", "Afrikaans", false)]
    [InlineData(SystemLanguage.Arabic, "ar", "العربية", "Arabic", false)]
    [InlineData(SystemLanguage.Basque, "eu", "euskara", "Basque", false)]
    [InlineData(SystemLanguage.Belarusian, "be", "беларуская", "Belarusian", false)]
    [InlineData(SystemLanguage.Bulgarian, "bg", "български", "Bulgarian", false)]
    [InlineData(SystemLanguage.Catalan, "ca", "català", "Catalan", false)]
    [InlineData(SystemLanguage.Czech, "cs", "čeština", "Czech", false)]
    [InlineData(SystemLanguage.Danish, "da", "dansk", "Danish", false)]
    [InlineData(SystemLanguage.Dutch, "nl", "Nederlands", "Dutch", false)]
    [InlineData(SystemLanguage.Estonian, "et", "eesti", "Estonian", false)]
    [InlineData(SystemLanguage.Faroese, "fo", "føroyskt", "Faroese", false)]
    [InlineData(SystemLanguage.Finnish, "fi", "suomi", "Finnish", false)]
    [InlineData(SystemLanguage.Greek, "el", "Ελληνικά", "Greek", false)]
    [InlineData(SystemLanguage.Hebrew, "he", "עברית", "Hebrew", false)]
    [InlineData(SystemLanguage.Hindi, "hi", "हिन्दी", "Hindi", false)]
    [InlineData(SystemLanguage.Hungarian, "hu", "magyar", "Hungarian", false)]
    [InlineData(SystemLanguage.Icelandic, "is", "íslenska", "Icelandic", false)]
    [InlineData(SystemLanguage.Indonesian, "id", "Indonesia", "Indonesian", false)]
    [InlineData(SystemLanguage.Latvian, "lv", "latviešu", "Latvian", false)]
    [InlineData(SystemLanguage.Lithuanian, "lt", "lietuvių", "Lithuanian", false)]
    [InlineData(SystemLanguage.Norwegian, "no", "norsk", "Norwegian", false)]
    [InlineData(SystemLanguage.Romanian, "ro", "română", "Romanian", false)]
    [InlineData(SystemLanguage.Slovak, "sk", "slovenčina", "Slovak", false)]
    [InlineData(SystemLanguage.Slovenian, "sl", "slovenščina", "Slovenian", false)]
    [InlineData(SystemLanguage.Swedish, "sv", "svenska", "Swedish", false)]
    [InlineData(SystemLanguage.Thai, "th", "ไทย", "Thai", false)]
    [InlineData(SystemLanguage.Turkish, "tr", "Türkçe", "Turkish", false)]
    [InlineData(SystemLanguage.Ukrainian, "uk", "українська", "Ukrainian", false)]
    [InlineData(SystemLanguage.Vietnamese, "vi", "Tiếng Việt", "Vietnamese", false)]
    //
    //
    [InlineData(SystemLanguage.Unknown, "Unknown", "other languages", "other languages", false)]
    //
    // INFO: when it's generated via log out of the game
    //[InlineData(SystemLanguage.SerboCroatian, "SerboCroatian", "српски/hrvatski", "Serbian/Croatian", false)]
    // 
    // INFO: when it's the test is run
    [InlineData(SystemLanguage.SerboCroatian, "SerboCroatian", "srpski/hrvatski", "Serbian/Croatian", false)]
    public void GeneralTests(SystemLanguage systemLanguage,
                             string expectedId,
                             string expectedName,
                             string expectedNameEnglish,
                             bool expectedIsBuiltIn) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);
        MyLanguages languages = runtimeContainer.Languages;
        Assert.Equal(43, languages.LanguageCount);
        MyLanguage? language = languages.GetLanguage(systemLanguage);
        Assert.NotNull(language);
        Assert.Equal(expectedId, language.Id);
        Assert.Equal(expectedName, language.Name);
        Assert.Equal(expectedNameEnglish, language.NameEnglish);
        Assert.Equal(expectedIsBuiltIn, language.IsBuiltIn);
        Assert.NotEmpty(language.CultureInfos);
        {
            // the following method i´s intended to get a language via a correct locale-id
            // whenever a flavor is changed
            // the SystemLanguages Unknown and SerboCroation have a general ID
            // that does not work with that method!
            if (systemLanguage is SystemLanguage.Unknown or SystemLanguage.SerboCroatian) {
                Assert.Null(languages.GetLanguage(expectedId));
            } else {
                Assert.NotNull(languages.GetLanguage(expectedId));
            }
            // no files are read
            Assert.False(language.HasFlavors);
            Assert.False(language.HasFlavor(expectedId));
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
    public void OkTests(SystemLanguage systemLanguage,
                        int expectedFlavorCount) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);
        MyLanguages languages = runtimeContainer.Languages;
        languages.Init();
        Assert.False(languages.HasErroneous);
        Assert.Equal(43, languages.LanguageCount);
        MyLanguage? language = languages.GetLanguage(systemLanguage);
        Assert.NotNull(language);
        Assert.True(language.HasFlavors);
        Assert.Equal(expectedFlavorCount, language.FlavorCount);
        Assert.Equal(expectedFlavorCount * this.dataProvider.EntryCountPerFile,
                     language.EntryCountOfAllFlavors);

        TestLocManager locManager = runtimeContainer.TestLocManager;
        if (language.IsBuiltIn) {
            // built in are loaded by the game itself
            // and are skipped within MyLanguages.Load(), cause they are loaded by the game itself
            Assert.False(locManager.SupportsLocale(language.Id));
            Assert.False(testLogProvider.HasLoggedTrace);
            Assert.False(testLogProvider.HasLoggedInfo);
            Assert.False(testLogProvider.HasLoggedWarning);
            Assert.False(testLogProvider.HasLoggedError);
            Assert.False(testLogProvider.HasLoggedCritical);
            Assert.False(testLogProvider.HasDisplayedError);
        } else {
            for (int i = 0; i < 2; i++) {
                if (i == 1) {
                    languages.ReLoad();
                }
                Assert.True(locManager.SupportsLocale(language.Id));
                Assert.True(locManager.Locales.ContainsKey(language.Id));
                bool gotLocale = locManager.Locales.TryGetValue(language.Id, out SystemLanguage locale);
                Assert.True(gotLocale);
                Assert.Equal(language.SystemLanguage, locale);
                Assert.True(locManager.LocaleNames.ContainsKey(language.Id));
                bool gotLocaleName = locManager.LocaleNames.TryGetValue(language.Id, out string localeName);
                Assert.True(gotLocaleName);
                Assert.Equal(language.Name, localeName);
                Assert.True(locManager.Sources.ContainsKey(language.Id));
                bool gotSources = locManager.Sources.TryGetValue(language.Id, out IList<IDictionarySource> sources);
                Assert.True(gotSources);
                Assert.Single(sources);
                Assert.IsType<TranslationFileSource>(sources[0]);
                Assert.False(testLogProvider.HasLoggedTrace);
                Assert.False(testLogProvider.HasLoggedInfo);
                Assert.False(testLogProvider.HasLoggedWarning);
                Assert.False(testLogProvider.HasLoggedError);
                Assert.False(testLogProvider.HasLoggedCritical);
                Assert.False(testLogProvider.HasDisplayedError);
            }
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
        TestDataProvider testDataProvider = new TestDataProvider {
            DirectoryName = nameof(OkNotTests)
        };
        try {
            ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
            testDataProvider.GenerateData();
            ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                                   userDataPath: testDataProvider.DirectoryName);
            MyLanguages languages = runtimeContainer.Languages;
            languages.Init();
            Assert.False(languages.HasErroneous);
            Assert.Equal(43, languages.LanguageCount);
            MyLanguage? language = languages.GetLanguage(systemLanguage);
            Assert.NotNull(language);
            Assert.True(language.HasFlavors);
            Assert.Equal(expectedFlavorCount, language.FlavorCount);
            // first its ok, so we expect two times expectedFlavorCount
            Assert.Equal(expectedFlavorCount * testDataProvider.EntryCountPerFile,
                         language.EntryCountOfAllFlavors);

            TestLocManager locManager = runtimeContainer.TestLocManager;
            if (language.IsBuiltIn) {
                // built in are loaded by the game itself
                // and are skipped within MyLanguages.Load(), cause they are loaded by the game itself
                Assert.False(locManager.SupportsLocale(language.Id));
                Assert.False(testLogProvider.HasLoggedTrace);
                Assert.False(testLogProvider.HasLoggedInfo);
                Assert.False(testLogProvider.HasLoggedWarning);
                Assert.False(testLogProvider.HasLoggedCritical);
                Assert.False(testLogProvider.HasDisplayedError);
                Assert.False(testLogProvider.HasLoggedError);
            } else {
                for (int i = 0; i < 2; i++) {
                    if (i == 1) {
                        testDataProvider.GenerateCorruptData();
                        languages.ReLoad();
                    }
                    Assert.True(locManager.SupportsLocale(language.Id));
                    Assert.True(locManager.Locales.ContainsKey(language.Id));
                    bool gotLocale = locManager.Locales.TryGetValue(language.Id, out SystemLanguage locale);
                    Assert.True(gotLocale);
                    Assert.Equal(language.SystemLanguage, locale);
                    Assert.True(locManager.LocaleNames.ContainsKey(language.Id));
                    bool gotLocaleName = locManager.LocaleNames.TryGetValue(language.Id, out string localeName);
                    Assert.True(gotLocaleName);
                    Assert.Equal(language.Name, localeName);
                    Assert.True(locManager.Sources.ContainsKey(language.Id));
                    bool gotSources = locManager.Sources.TryGetValue(language.Id, out IList<IDictionarySource> sources);
                    Assert.True(gotSources);
                    Assert.Single(sources);
                }
                Assert.False(testLogProvider.HasLoggedTrace);
                Assert.False(testLogProvider.HasLoggedWarning);
                Assert.False(testLogProvider.HasLoggedInfo);
                Assert.False(testLogProvider.HasLoggedCritical);
                Assert.False(testLogProvider.HasDisplayedError);
                //
                Assert.True(testLogProvider.HasLoggedError);
            }
        } finally {
            testDataProvider.Dispose();
        }
    }
    [Fact]
    public void LogMarkdownAndCultureInfoNamesTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<MyLanguagesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
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
}
