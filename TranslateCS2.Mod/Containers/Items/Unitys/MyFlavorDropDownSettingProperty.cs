using System.Collections.Generic;

using Colossal.Json;

using Game.Settings;
using Game.UI.Menu;
using Game.UI.Widgets;

using TranslateCS2.Mod.Helpers;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorDropDownSettingProperty : AutomaticSettings.ManualProperty {
    private readonly MyLanguage language;
    public MyFlavorDropDownSettingProperty(MyLanguage language,
                                           ModSettings modSettings,
                                           string name) : base(modSettings.GetType(),
                                                               typeof(string),
                                                               name) {
        this.language = language;
        this.SetReadWrite();
        this.SetGetterSetter();
        this.AddAttributes();
    }

    private void SetReadWrite() {
        this.canRead = true;
        this.canWrite = true;
    }

    private void SetGetterSetter() {
        this.setter = this.Setter;
        this.getter = this.Getter;
    }

    private void Setter(object modSettings, object flavorId) {
        ((ModSettings) modSettings).SetFlavor(this.language.SystemLanguage, flavorId);
    }

    private string Getter(object modSettings) {
        return ((ModSettings) modSettings).GetSettedFlavor(this.language.SystemLanguage);
    }

    private void AddAttributes() {
        this.attributes.Add(new ExcludeAttribute());
        // instance methods are only allowed, if itemsGetterType is an instance of ModSettings
        /// <see cref="AutomaticSettings.SettingItemData.TryGetAction{T}(Setting, System.Type, System.String, out System.Func{T})"/>
        // the attribute is added, cause there might be sideeffects, if its left
        // it has to be added with this.GetType(), cause GetFlavors-method is within this class
        // this is not an instance of ModSettings,
        // therefore, it is handled over there:
        /// <see cref="MyFlavorDropDownSettingItemData.GetWidget"/>
        this.attributes.Add(new SettingsUIDropdownAttribute(this.GetType(), nameof(GetFlavors)));
    }

    public DropdownItem<string>[] GetFlavors() {
        // only builtin and those without flavors may have 'none'
        bool addNone = this.language.IsBuiltIn || !this.language.HasFlavorsWithSources;
        List<DropdownItem<string>> flavors = DropDownItemsHelper.GetDefault(addNone);
        flavors.AddRange(this.language.GetFlavorDropDownItems());
        return flavors.ToArray();
    }
}
