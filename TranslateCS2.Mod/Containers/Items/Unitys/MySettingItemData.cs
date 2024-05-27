using Game.Settings;
using Game.UI.Menu;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MySettingItemData : AutomaticSettings.SettingItemData {
    public SystemLanguage SystemLanguage { get; }
    public MySettingItemData(AutomaticSettings.WidgetType widgetType,
                             Setting setting,
                             AutomaticSettings.IProxyProperty property,
                             SystemLanguage systemLanguage) : base(widgetType,
                                                                   setting,
                                                                   property) {
        this.SystemLanguage = systemLanguage;
    }
}
