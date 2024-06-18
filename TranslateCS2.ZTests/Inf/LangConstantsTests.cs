using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class LangConstantsTests {
    [Fact]
    public void ChineseSimplifiedTest() {
        Assert.Equal("Chinese (Simplified", LangConstants.ChineseSimplified);
    }
    [Fact]
    public void ChineseTraditionalTest() {
        Assert.Equal("Chinese (Traditional", LangConstants.ChineseTraditional);
    }
    [Fact]
    public void CroatianTest() {
        Assert.Equal("Croatian", LangConstants.Croatian);
    }
    [Fact]
    public void SerbianTest() {
        Assert.Equal("Serbian", LangConstants.Serbian);
    }
    [Fact]
    public void LatinTest() {
        Assert.Equal("Latin", LangConstants.Latin);
    }
    [Fact]
    public void CyrillicTest() {
        Assert.Equal("Cyrillic", LangConstants.Cyrillic);
    }
    [Fact]
    public void OtherLanguagesTest() {
        Assert.Equal("other languages", LangConstants.OtherLanguages);
    }
    [Fact]
    public void OtherLanguagesSelectTest() {
        Assert.Equal("Select a language.", LangConstants.OtherLanguagesSelect);
    }
}
