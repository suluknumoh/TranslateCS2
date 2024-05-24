using System.Collections.Generic;

using Game.UI.Widgets;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Containers.Items;
public class DropDownItems {
    public static string None { get; } = StringConstants.NoneLower;
    internal DropDownItems() { }
    public List<DropdownItem<string>> GetDefault(bool addNone) {
        List<DropdownItem<string>> flavors = [];
        if (addNone) {
            flavors.Add(new DropdownItem<string>() {
                value = None,
                displayName = None
            });
        }
        return flavors;
    }
}
