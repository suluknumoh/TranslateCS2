using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Mod.Containers.Items;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class DropDownItemsTests {
    [Fact]
    public void NoneTest() {
        Assert.Equal("none", DropDownItems.None);
    }
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 0)]
    public void GetDefaultTest(bool addNone, int expectedSize) {
        DropDownItems items = new DropDownItems();
        List<DropdownItem<string>> result = items.GetDefault(addNone);
        Assert.Equal(expectedSize, result.Count);
    }
}
