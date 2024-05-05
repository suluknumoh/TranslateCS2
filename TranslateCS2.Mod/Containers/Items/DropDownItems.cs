using System.Collections.Generic;

using Game.UI.Widgets;

namespace TranslateCS2.Mod.Containers.Items;
public class DropDownItems {
    public static string None { get; } = nameof(None).ToLower();
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
