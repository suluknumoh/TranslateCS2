using Game.Settings;
using Game.UI.Menu;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorSettingItemData : AutomaticSettings.SettingItemData {
    public SystemLanguage SystemLanguage { get; }
    public MyFlavorSettingItemData(AutomaticSettings.WidgetType widgetType,
                                   Setting setting,
                                   AutomaticSettings.IProxyProperty property,
                                   SystemLanguage systemLanguage) : base(widgetType,
                                                                         setting,
                                                                         property) {
        this.SystemLanguage = systemLanguage;
    }
}
