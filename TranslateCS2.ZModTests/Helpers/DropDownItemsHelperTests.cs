using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Mod.Helpers;

using Xunit;

namespace TranslateCS2.ZModTests.Helpers;
public class DropDownItemsHelperTests {
    [Fact]
    public void NoneTest() {
        Assert.Equal("none", DropDownItemsHelper.None);
    }
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void GetDefaultTest(bool addNone, int expectedSize) {
        List<DropdownItem<string>> result = DropDownItemsHelper.GetDefault(addNone);
        Assert.Equal(expectedSize, result.Count);
    }
}
