using System;

using Game.Settings;
using Game.UI.Menu;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorDropDownSettingItemData : AutomaticSettings.SettingItemData {
    private readonly IModRuntimeContainer runtimeContainer;
    public MyLanguage Language { get; }
    public SystemLanguage SystemLanguage { get; }
    public MyFlavorDropDownSettingItemData(Setting setting,
                                           AutomaticSettings.IProxyProperty property,
                                           IModRuntimeContainer runtimeContainer,
                                           MyLanguage language) : base(AutomaticSettings.WidgetType.StringDropdown,
                                                                       setting,
                                                                       property) {
        this.runtimeContainer = runtimeContainer;
        this.Language = language;
        this.SystemLanguage = language.SystemLanguage;
        this.hideAction = this.IsHidden;
        this.disableAction = this.IsDisabled;
    }
    public bool IsHidden() {
        return
            !this.Language.Id.Equals(this.runtimeContainer.IntSettings.CurrentLocale, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsDisabled() {
        return
            !this.Language.Id.Equals(this.runtimeContainer.IntSettings.CurrentLocale, StringComparison.OrdinalIgnoreCase)
            || !this.Language.HasFlavors;
    }
}
