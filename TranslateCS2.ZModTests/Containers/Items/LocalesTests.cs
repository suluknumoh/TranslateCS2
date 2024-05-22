using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using UnityEngine;

namespace TranslateCS2.ZModTests.Containers.Items;
public class LocalesTests {
    [Theory]
    [InlineData("de-DE", true)]
    [InlineData("en-US", true)]
    [InlineData("es-ES", true)]
    [InlineData("fr-FR", true)]
    [InlineData("it-IT", true)]
    [InlineData("ja-JP", true)]
    [InlineData("ko-KR", true)]
    [InlineData("pl-PL", true)]
    [InlineData("pt-BR", true)]
    [InlineData("ru-RU", true)]
    [InlineData("zh-HANS", true)]
    [InlineData("zh-HANT", true)]
    [InlineData("De-De", true)]
    [InlineData("En-Us", true)]
    [InlineData("Es-Es", true)]
    [InlineData("Fr-Fr", true)]
    [InlineData("It-It", true)]
    [InlineData("Ja-Jp", true)]
    [InlineData("Ko-Kr", true)]
    [InlineData("Pl-Pl", true)]
    [InlineData("Pt-Br", true)]
    [InlineData("Ru-Ru", true)]
    [InlineData("Zh-Hans", true)]
    [InlineData("Zh-Hant", true)]
    [InlineData("de", false)]
    [InlineData("en", false)]
    [InlineData("es", false)]
    [InlineData("fr", false)]
    [InlineData("it", false)]
    [InlineData("ja", false)]
    [InlineData("ko", false)]
    [InlineData("pl", false)]
    [InlineData("pt", false)]
    [InlineData("ru", false)]
    [InlineData("zh", false)]

    public void IsBuiltInTest(string localeId, bool expectedIsBuiltIn) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<LocalesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider);
        Locales locales = runtimeContainer.Locales;
        Assert.NotNull(locales);
        bool isBuiltIn = locales.IsBuiltIn(localeId);
        Assert.Equal(expectedIsBuiltIn, isBuiltIn);
    }
    [Theory]
    [InlineData("De-De", "de-DE")]
    [InlineData("En-Us", "en-US")]
    [InlineData("Es-Es", "es-ES")]
    [InlineData("Fr-Fr", "fr-FR")]
    [InlineData("It-It", "it-IT")]
    [InlineData("Ja-Jp", "ja-JP")]
    [InlineData("Ko-Kr", "ko-KR")]
    [InlineData("Pl-Pl", "pl-PL")]
    [InlineData("Pt-Br", "pt-BR")]
    [InlineData("Ru-Ru", "ru-RU")]
    [InlineData("Zh-Hans", "zh-HANS")]
    [InlineData("Zh-Hant", "zh-HANT")]
    [InlineData("de", "de")]
    [InlineData("en", "en")]
    [InlineData("es", "es")]
    [InlineData("fr", "fr")]
    [InlineData("it", "it")]
    [InlineData("ja", "ja")]
    [InlineData("ko", "ko")]
    [InlineData("pl", "pl")]
    [InlineData("pt", "pt")]
    [InlineData("ru", "ru")]
    [InlineData("zh", "zh")]
    public void CorrectLocaleIdTest(string input, string expectedOutput) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<LocalesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider);
        Locales locales = runtimeContainer.Locales;
        Assert.NotNull(locales);
        string output = locales.CorrectLocaleId(input);
        Assert.Equal(expectedOutput, output);
    }
    [Fact]
    public void GetSystemLanguageCulturesMappingTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<LocalesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider);
        Locales locales = runtimeContainer.Locales;
        Assert.NotNull(locales);
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        IDictionary<SystemLanguage, IList<CultureInfo>> systemLanguageCulturesMapping = locales.GetSystemLanguageCulturesMapping();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            bool containsKey = systemLanguageCulturesMapping.ContainsKey(systemLanguage);
            if (SystemLanguage.Chinese == systemLanguage) {
                Assert.False(containsKey);
                continue;
            }
            Assert.True(containsKey);
            bool got = systemLanguageCulturesMapping.TryGetValue(systemLanguage, out IList<CultureInfo> systemLanguageCultures);
            Assert.True(got);
            Assert.NotEmpty(systemLanguageCultures);
        }
    }
}
