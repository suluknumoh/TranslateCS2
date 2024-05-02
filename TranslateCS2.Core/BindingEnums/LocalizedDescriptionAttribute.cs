using System;
using System.ComponentModel;
using System.Resources;

using TranslateCS2.Inf;

namespace TranslateCS2.Core.BindingEnums;
/// <seealso href="https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/"/>
/// <seealso href="https://github.com/brianlagunas/BindingEnumsInWpf"/>
public class LocalizedDescriptionAttribute : DescriptionAttribute {
    private readonly ResourceManager _resourceManager;
    private readonly string _resourceKey;

    public LocalizedDescriptionAttribute(string resourceKey, Type resourceType) {
        this._resourceManager = new ResourceManager(resourceType);
        this._resourceKey = resourceKey;
    }

    public override string Description {
        get {
            string? description = this._resourceManager.GetString(this._resourceKey);
            if (description != null
                && !StringHelper.IsNullOrWhiteSpaceOrEmpty(description)) {
                return description;
            }
            return String.Format("[[{0}]]", this._resourceKey);
        }
    }
}
