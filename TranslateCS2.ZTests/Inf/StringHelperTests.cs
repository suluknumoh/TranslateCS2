using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class StringHelperTests {
    [Theory]
    [InlineData("", true)]
    [InlineData(" ", true)]
    [InlineData(null, true)]
    [InlineData("null", false)]
    public void IsNullOrWhiteSpaceOrEmptyTest(string? s, bool expected) {
        bool result = StringHelper.IsNullOrWhiteSpaceOrEmpty(s);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("", null)]
    [InlineData(" ", null)]
    [InlineData(null, null)]
    [InlineData("null", "null")]
    public void GetNullForEmptyTest(string? s, string? expected) {
        string? result = StringHelper.GetNullForEmpty(s);
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData("", 0, "")]
    [InlineData(" ", 0, "...")]
    [InlineData("mustertext ist schwer zu erfinden", 0, "...")]
    [InlineData("mustertext ist schwer zu erfinden", 10, "mustertext...")]
    [InlineData("mustertext ist schwer zu erfinden", 11, "mustertext ...")]
    [InlineData("mustertext ist schwer zu erfinden", -1, "mustertext ist schwer zu erfinden")]
    public void CutStringAfterMaxLengthAndAddThreeDotsTest(string input, int maxLen, string expected) {
        string output = StringHelper.CutStringAfterMaxLengthAndAddThreeDots(input, maxLen);
        Assert.Equal(expected, output);
    }
}
