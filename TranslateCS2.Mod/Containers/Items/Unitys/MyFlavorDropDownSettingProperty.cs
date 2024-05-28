using System.Collections.Generic;

using Colossal.Json;

using Game.Settings;
using Game.UI.Menu;
using Game.UI.Widgets;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
internal class MyFlavorDropDownSettingProperty : AutomaticSettings.ManualProperty {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly MyLanguage language;
    private readonly ModSettings modSettings;
    public MyFlavorDropDownSettingProperty(IModRuntimeContainer runtimeContainer,
                                           MyLanguage language,
                                           ModSettings modSettings,
                                           string name) : base(modSettings.GetType(),
                                                               typeof(string),
                                                               name) {
        this.runtimeContainer = runtimeContainer;
        this.language = language;
        this.modSettings = modSettings;
        this.canRead = true;
        this.canWrite = true;
        this.setter = (modSettings, localeId) => ((ModSettings) modSettings).Setter(language.SystemLanguage, localeId);
        this.getter = (modSettings) => ((ModSettings) modSettings).Getter(language.SystemLanguage);
        this.attributes.Add(new ExcludeAttribute());
        // instance methods are only allowed, if itemsGetterType is an instance of ModSettings
        /// <see cref="AutomaticSettings.SettingItemData.TryGetAction{T}(Setting, System.Type, System.String, out System.Func{T})"/>
        this.attributes.Add(new SettingsUIDropdownAttribute(this.GetType(), nameof(GetFlavors)));
    }
    public DropdownItem<string>[] GetFlavors() {
        MyLanguage language = this.language;
        List<DropdownItem<string>> flavors = this.runtimeContainer.DropDownItems.GetDefault(true);
        if (language != null) {
            // only builtin and those without flavors may have 'none'
            bool addNone = language.IsBuiltIn || !language.HasFlavors;
            flavors = this.runtimeContainer.DropDownItems.GetDefault(addNone);
            flavors.AddRange(language.GetFlavorDropDownItems());
        }
        return flavors.ToArray();
    }
}
