using System.Collections;

namespace TranslateCS2.Core.Helpers.Ribbons;
public class RibbonComboBoxItems {
    public string? SeparatorLabel { get; set; }
    public IList Items { get; }
    public RibbonComboBoxItems(IList items) {
        this.Items = items;
    }
}
