using TranslateCS2.Inf;

namespace TranslateCS2.ZTests.Inf;
public class StringConstantsTests {
    [Fact]
    public void UnderscoreTest() {
        Assert.Equal("_", StringConstants.Underscore);
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
}
