using System.Collections.Generic;

using Game.UI.Widgets;

namespace TranslateCS2.Mod.Helpers;
internal static class DropDownItemsHelper {
    public static string None { get; } = nameof(None).ToLower();
    public static List<DropdownItem<string>> GetDefault(bool addNone) {
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
