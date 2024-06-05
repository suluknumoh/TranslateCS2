using System;

using Game.UI.Menu;
using Game.UI.Widgets;


namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorDropDownSettingItemData : AutomaticSettings.SettingItemData {
    public MyLanguage Language { get; }
    private MyFlavorDropDownSettingItemData(MyFlavorDropDownSettingProperty property,
                                            ModSettings modSettings,
                                            MyLanguage language) : base(AutomaticSettings.WidgetType.StringDropdown,
                                                                        modSettings,
                                                                        property) {
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
        return !this.Language.IsCurrent();
    }

    public bool IsDisabled() {
        return
            this.IsHidden()
            || !this.Language.HasFlavors;
    }
    protected override IWidget GetWidget() {
        // instance methods are only allowed, if itemsGetterType is an instance of ModSettings
        /// <see cref="AutomaticSettings.SettingItemData.TryGetAction{T}(Setting, Type, String, out Func{T})"/>
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
    public void UpdateWidget() {
        if (this.widget is null) {
            return;
        }
        this.widget.Update();
        // INFO: it is not possible, to add languages at runtime; would require access to Game.Settings.InterfaceSettings PageData that is unreachable(?) for this mod
        //
        // TODO: WWW-0: is it now possible to have only one flavor-drop-down???
        // TODO: WWW-1: changing items based on selected locale...???
        // TODO: WWW-2: this mod can hold the instance of the single drop down
        // TODO: WWW-3: Game.UI.Widgets.DropdownField<T>.itemsVersion
        // TODO: WWW-4: is a function, that is called while updating, does it trigger something to refresh???
    }
    public static MyFlavorDropDownSettingItemData Create(MyLanguage language,
                                                         ModSettings modSettings,
                                                         string propertyName) {
        MyFlavorDropDownSettingProperty property = new MyFlavorDropDownSettingProperty(language,
                                                                                       modSettings,
                                                                                       propertyName);
        MyFlavorDropDownSettingItemData item = new MyFlavorDropDownSettingItemData(property,
                                                                                   modSettings,
                                                                                   language);
        return item;
    }
}
