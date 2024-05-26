using System.Collections.Generic;
using System.Globalization;

using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class CultureInfoHelperTests {
    [Theory]
    [InlineData("German", true, true)]
    [InlineData("English", true, true)]
    public void GeneralTest(string nameEnglish, bool expectedHasNeutral, bool expectedHasSpecific) {
        IEnumerable<CultureInfo>? cultureInfos = CultureInfoHelper.GatherCulturesFromEnglishName(nameEnglish);
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
}
