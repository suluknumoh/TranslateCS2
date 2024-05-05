using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Colossal.IO.AssetDatabase.Internal;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
public class MyLanguage {
    private readonly IModRuntimeContainer runtimeContainer;
    public string ID { get; private set; }
    public string Name { get; private set; }
    public string NameEnglish { get; private set; }
    internal IList<TranslationFile> Flavors { get; } = [];
    public int FlavorCount => this.Flavors.Count;
    public int EntryCountOfAllFlavors {
        get {
            int count = 0;
            if (this.HasFlavors) {
                this.Flavors.ForEach(flavor => count += flavor.EntryCount);
            }
            return count;
        }
    }
    public List<CultureInfo> CultureInfos { get; } = [];
    public SystemLanguage SystemLanguage { get; }
    public bool IsBuiltIn { get; private set; }
    public bool HasFlavors => this.Flavors.Any();
    internal MyLanguage(SystemLanguage systemLanguage, IModRuntimeContainer runtimeContainer) {
        this.SystemLanguage = systemLanguage;
        this.runtimeContainer = runtimeContainer;
    }
    public void Init() {
        IEnumerable<CultureInfo> builtin = this.CultureInfos.Where(ci => this.runtimeContainer.Locales.IsBuiltIn(ci.Name));
        if (builtin.Any()) {
            CultureInfo ci = builtin.First();
            this.ID = this.runtimeContainer.Locales.CorrectLocaleId(ci.Name);
            if (this.SystemLanguage == SystemLanguage.Portuguese) {
                this.Name = ci.NativeName;
                this.NameEnglish = ci.EnglishName;
            } else {
                this.Name = ci.Parent.NativeName;
                this.NameEnglish = ci.Parent.EnglishName;
            }
            this.IsBuiltIn = true;
        } else {
            IEnumerable<CultureInfo> remaining = this.CultureInfos.Where(ci => !ci.Name.Contains("-"));
            if (remaining.Any()) {
                CultureInfo ci = remaining.First();
                this.ID = this.runtimeContainer.Locales.CorrectLocaleId(ci.Name);
                if (SystemLanguage.SerboCroatian == this.SystemLanguage) {
                    this.ID = this.SystemLanguage.ToString();
                    this.Name = String.Join("/", remaining.OrderByDescending(ci => ci.Name).Select(ci => ci.NativeName));
                    this.NameEnglish = String.Join("/", remaining.OrderByDescending(ci => ci.Name).Select(ci => ci.EnglishName));
                } else {
                    this.Name = ci.NativeName;
                    this.NameEnglish = ci.EnglishName;
                }
                this.IsBuiltIn = false;
            }
        }
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{nameof(MyLanguage)}");
        builder.AppendLine($"{nameof(this.ID)}: {this.ID}");
        builder.AppendLine($"{nameof(this.NameEnglish)}: {this.NameEnglish}");
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.AppendLine($"{nameof(this.IsBuiltIn)}: {this.IsBuiltIn}");
        builder.AppendLine($"{nameof(this.HasFlavors)}: {this.HasFlavors}");
        builder.AppendLine($"{nameof(this.CultureInfos)}: ({String.Join(", ", this.CultureInfos)})");
        return builder.ToString();
    }

    public CultureInfo? GetCultureInfo(string localeId) {
        IEnumerable<CultureInfo> matches = this.CultureInfos.Where(item => item.Name.ToLower() == localeId.ToLower());
        if (matches.Any()) {
            return matches.First();
        }
        return null;
    }

    public IEnumerable<DropdownItem<string>> GetFlavorDropDownItems() {
        List<DropdownItem<string>> dropdownItems = [];
        foreach (TranslationFile translationFile in this.Flavors) {
            if (translationFile.LocaleId == null || translationFile.LocaleName == null) {
                continue;
            }
            string displayName = translationFile.LocaleName;
            if (displayName.Length > ModConstants.MaxDisplayNameLength) {
                displayName = displayName.Substring(0, ModConstants.MaxDisplayNameLength);
                displayName += "...";
            }
            DropdownItem<string> item = new DropdownItem<string>() {
                value = translationFile.LocaleId,
                displayName = displayName
            };
            dropdownItems.Add(item);
        }
        return dropdownItems;
    }

    public bool HasFlavor(string localeId) {
        return this.Flavors.Where(item => item.LocaleId.Equals(localeId, StringComparison.OrdinalIgnoreCase)).Any();
    }

    internal TranslationFile GetFlavor(string localeId) {
        return this.Flavors.Where(item => item.LocaleId.Equals(localeId, StringComparison.OrdinalIgnoreCase)).First();
    }
}
