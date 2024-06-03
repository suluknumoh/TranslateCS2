using System.Collections.Generic;
using System.Globalization;

using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class CultureInfoHelperTests {
    [Theory]
    [InlineData(null, false, false)]
    [InlineData("German", true, true)]
    [InlineData("English", true, true)]
    public void GeneralTest(string? nameEnglish, bool expectedHasNeutral, bool expectedHasSpecific) {
        IEnumerable<CultureInfo>? cultureInfos = CultureInfoHelper.GatherCulturesFromEnglishName(nameEnglish);
        if (nameEnglish is null) {
            Assert.Null(cultureInfos);
            return;
        }
        Assert.NotNull(cultureInfos);
        Assert.NotEmpty(cultureInfos);

        bool hasNeutral = CultureInfoHelper.HasNeutralCultures(cultureInfos);
        Assert.Equal(expectedHasNeutral, hasNeutral);
        IEnumerable<CultureInfo> neutrals = CultureInfoHelper.GetNeutralCultures(cultureInfos);
        Assert.Single(neutrals);

        bool hasSpecific = CultureInfoHelper.HasSpecificCultures(cultureInfos);
        Assert.Equal(expectedHasSpecific, hasSpecific);
        IEnumerable<CultureInfo> specifics = CultureInfoHelper.GetSpecificCultures(cultureInfos);
        Assert.NotEmpty(specifics);
    }
    [Theory]
    [InlineData(null, null)]
    [InlineData("null", "null")]
    [InlineData("De", "de")]
    [InlineData("dE", "de")]
    [InlineData("zH-hAnS", "zh-Hans")]
    [InlineData("zH-hAnT", "zh-Hant")]
    public void CorrectLocaleIdTest(string? input, string? expected) {
        string? result = CultureInfoHelper.CorrectLocaleId(input);
        Assert.Equal(expected, result);
    }
}
