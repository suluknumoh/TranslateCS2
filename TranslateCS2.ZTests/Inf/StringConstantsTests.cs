using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class StringConstantsTests {
    [Fact]
    public void UnderscoreTest() {
        Assert.Equal("_", StringConstants.Underscore);
    }
    [Fact]
    public void UnderscoreCharTest() {
        Assert.Equal('_', StringConstants.UnderscoreChar);
    }
    [Fact]
    public void SpaceTest() {
        Assert.Equal(" ", StringConstants.Space);
    }
    [Fact]
    public void LowTest() {
        Assert.Equal("Low", StringConstants.Low);
    }
    [Fact]
    public void Colossal_OrderTest() {
        Assert.Equal("Colossal Order", StringConstants.Colossal_Order);
    }
    [Fact]
    public void Cities_Skylines_IITest() {
        Assert.Equal("Cities Skylines II", StringConstants.Cities_Skylines_II);
    }
    [Fact]
    public void ForwardSlashTest() {
        Assert.Equal("/", StringConstants.ForwardSlash);
    }
    [Fact]
    public void BackSlashTest() {
        Assert.Equal("\\", StringConstants.BackSlash);
    }
    [Fact]
    public void BackSlashDoubleTest() {
        Assert.Equal("\\\\", StringConstants.BackSlashDouble);
    }
    [Fact]
    public void DotTest() {
        Assert.Equal(".", StringConstants.Dot);
    }
    [Fact]
    public void ThreeDotsTest() {
        Assert.Equal("...", StringConstants.ThreeDots);
    }
    [Fact]
    public void DashTest() {
        Assert.Equal("-", StringConstants.Dash);
    }
    [Fact]
    public void DataTildeTest() {
        Assert.Equal("Data~", StringConstants.DataTilde);
    }
    [Fact]
    public void NoneTest() {
        Assert.Equal("None", StringConstants.None);
    }
    [Fact]
    public void NoneLowerTest() {
        Assert.Equal("none", StringConstants.NoneLower);
    }
    [Fact]
    public void CarriageReturnTest() {
        Assert.Equal("\r", StringConstants.CarriageReturn);
    }
    [Fact]
    public void LineFeedTest() {
        Assert.Equal("\n", StringConstants.LineFeed);
    }
    [Fact]
    public void TabTest() {
        Assert.Equal("\t", StringConstants.Tab);
    }
    [Fact]
    public void QuotationMarkTest() {
        Assert.Equal("\"", StringConstants.QuotationMark);
    }
    [Fact]
    public void CommaSpaceTest() {
        Assert.Equal(", ", StringConstants.CommaSpace);
    }
    [Fact]
    public void LocalModTest() {
        Assert.Equal("LocalMod", StringConstants.LocalMod);
    }
    [Fact]
    public void UnofficialLocalesTest() {
        Assert.Equal("UnofficialLocales", StringConstants.UnofficialLocales);
    }
    [Fact]
    public void AllTest() {
        Assert.Equal("All", StringConstants.All);
    }
}
