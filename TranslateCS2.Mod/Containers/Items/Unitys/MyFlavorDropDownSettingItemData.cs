using System;

using Game.Settings;
using Game.UI.Menu;
using Game.UI.Widgets;


namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorDropDownSettingItemData : AutomaticSettings.SettingItemData {
    private readonly IModRuntimeContainer runtimeContainer;
    public MyLanguage Language { get; }
    public MyFlavorDropDownSettingItemData(IModRuntimeContainer runtimeContainer,
                                           MyFlavorDropDownSettingProperty property,
                                           Setting setting,
                                           MyLanguage language) : base(AutomaticSettings.WidgetType.StringDropdown,
                                                                       setting,
                                                                       property) {
        this.runtimeContainer = runtimeContainer;
        this.Language = language;
        this.hideAction = this.IsHidden;
        this.disableAction = this.IsDisabled;
        this.simpleGroup = ModSettings.FlavorGroup;
        // setterAction would be a method/an action that takes a string as parameter
        /// <see cref="AutomaticSettings.AddStringDropdownProperty(AutomaticSettings.SettingItemData)"/>
        // it is not needed,
        // cause it is handled via
        /// <see cref="MyFlavorDropDownSettingProperty.setter"/>
        // and
        /// <see cref="MyFlavorDropDownSettingProperty.getter"/>
        this.setterAction = null;
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
    protected override IWidget GetWidget() {
        // instance methods are only allowed, if itemsGetterType is an instance of ModSettings
        /// <see cref="AutomaticSettings.SettingItemData.TryGetAction{T}(Setting, System.Type, System.String, out System.Func{T})"/>
        //
        // first colossal orders defaults
        IWidget widget = base.GetWidget();
        //
        //
        if (this.property is MyFlavorDropDownSettingProperty myProperty
            && widget is DropdownField<string> dropDown
            && dropDown.itemsAccessor is null) {
            //
            //
            //
            Func<DropdownItem<string>[]> action = myProperty.GetFlavors;
            dropDown.itemsAccessor = new AutomaticSettings.DropdownItemsAccessor<DropdownItem<string>[]>(action);
            //
            //
            //
        }
        //
        //
        return widget;
    }
}
