using TranslateCS2.Inf;

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
}
