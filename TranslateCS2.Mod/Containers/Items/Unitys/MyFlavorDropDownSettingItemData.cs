using System;

using Game.UI.Menu;
using Game.UI.Widgets;


namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorDropDownSettingItemData : AutomaticSettings.SettingItemData {
    public MyLanguage Language { get; }
    private MyFlavorDropDownSettingItemData(MyFlavorDropDownSettingProperty property,
                                            ModSettings modSettings,
                                            MyLanguage language,
                                            string prefix) : base(AutomaticSettings.WidgetType.StringDropdown,
                                                                  modSettings,
                                                                  property,
                                                                  prefix) {
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
    }
    public static MyFlavorDropDownSettingItemData Create(MyLanguage language,
                                                         ModSettings modSettings,
                                                         string propertyName,
                                                         string prefix) {
        MyFlavorDropDownSettingProperty property = new MyFlavorDropDownSettingProperty(language,
                                                                                       modSettings,
                                                                                       propertyName);
        MyFlavorDropDownSettingItemData item = new MyFlavorDropDownSettingItemData(property,
                                                                                   modSettings,
                                                                                   language,
                                                                                   prefix);
        return item;
    }
}
