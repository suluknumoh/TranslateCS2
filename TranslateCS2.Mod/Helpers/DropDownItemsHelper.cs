using System.Collections.Generic;

using Game.UI.Widgets;

namespace TranslateCS2.Mod.Helpers;
internal static class DropDownItemsHelper {
    public static string None { get; } = "none";
    public static List<DropdownItem<string>> GetDefault() {
        List<DropdownItem<string>> flavors = [];
        flavors.Add(new DropdownItem<string>() {
            value = None,
            displayName = None
        });
        return flavors;
    }
}
