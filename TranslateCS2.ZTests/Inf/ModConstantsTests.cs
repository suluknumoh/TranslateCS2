using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class ModConstantsTests {
    [Fact]
    public void ModsDataTest() {
        Assert.Equal("ModsData", ModConstants.ModsData);
    }
    [Fact]
    public void ModsSettingsTest() {
        Assert.Equal("ModsSettings", ModConstants.ModsSettings);
    }
    [Fact]
    public void NameSimpleTest() {
        Assert.Equal("TranslateCS2", ModConstants.NameSimple);
    }
    [Fact]
    public void NameTest() {
        Assert.Equal("TranslateCS2.Mod", ModConstants.Name);
    }
    [Fact]
    public void ModIdTest() {
        Assert.Equal(79187, ModConstants.ModId);
    }
    [Fact]
    public void LocaleNameLocalizedKeyTest() {
        Assert.Equal("TranslateCS2.LocaleNameLocalizedKey", ModConstants.LocaleNameLocalizedKey);
    }
    [Fact]
    public void JsonExtensionTest() {
        Assert.Equal(".json", ModConstants.JsonExtension);
    }
    [Fact]
    public void JsonSearchPatternTest() {
        Assert.Equal("*.json", ModConstants.JsonSearchPattern);
    }
    [Fact]
    public void LocExtensionTest() {
        Assert.Equal(".loc", ModConstants.LocExtension);
    }
    [Fact]
    public void LocSearchPatternTest() {
        Assert.Equal("*.loc", ModConstants.LocSearchPattern);
    }
    [Fact]
    public void DllExtensionTest() {
        Assert.Equal(".dll", ModConstants.DllExtension);
    }
    [Fact]
    public void DllSearchPatternTest() {
        Assert.Equal("*.dll", ModConstants.DllSearchPattern);
    }
    [Fact]
    public void MaxDisplayNameLengthTest() {
        Assert.Equal(31, ModConstants.MaxDisplayNameLength);
    }
    [Fact]
    public void MaxErroneousTest() {
        Assert.Equal(5, ModConstants.MaxErroneous);
    }
    [Fact]
    public void ModExportKeyValueJsonNameTest() {
        Assert.Equal("_TranslateCS2.Mod.json", ModConstants.ModExportKeyValueJsonName);
    }
    [Fact]
    public void DataPathRawGeneralTest() {
        Assert.Equal("ModsData/", ModConstants.DataPathRawGeneral);
    }
    [Fact]
    public void DataPathRawSpecificTest() {
        Assert.Equal("ModsData/TranslateCS2.Mod/", ModConstants.DataPathRawSpecific);
    }
    [Fact]
    public void OtherModsLocFilePathTest() {
        Assert.Equal("UnofficialLocales", ModConstants.OtherModsLocFilePath);
    }
}
