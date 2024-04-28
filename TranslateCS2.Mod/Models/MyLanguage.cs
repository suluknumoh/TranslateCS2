﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Game.UI.Widgets;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class MyLanguage {
    public string ID { get; private set; }
    public string Name { get; private set; }
    public IList<TranslationFile> Flavors { get; } = [];
    public IList<CultureInfo> CultureInfos { get; } = [];
    public SystemLanguage SystemLanguage { get; }
    public bool IsBuiltIn { get; private set; }
    public bool HasFlavors => this.Flavors.Any();
    public MyLanguage(SystemLanguage systemLanguage) {
        this.SystemLanguage = systemLanguage;
    }
    public void Init() {
        IEnumerable<CultureInfo> builtin = this.CultureInfos.Where(ci => LocaleHelper.IsBuiltIn(ci.Name));
        if (builtin.Any()) {
            CultureInfo ci = builtin.First();
            this.ID = LocaleHelper.CorrectLocaleId(ci.Name);
            this.Name = ci.NativeName;
            this.IsBuiltIn = true;
        } else {
            IEnumerable<CultureInfo> remaining = this.CultureInfos.Where(ci => !ci.Name.Contains("-"));
            if (remaining.Any()) {
                CultureInfo ci = remaining.First();
                this.ID = LocaleHelper.CorrectLocaleId(ci.Name);
                if (SystemLanguage.SerboCroatian == this.SystemLanguage) {
                    // otherwise it would be Croatian only
                    this.Name = this.SystemLanguage.ToString();
                } else {
                    this.Name = ci.NativeName;
                }
                this.IsBuiltIn = false;
            }
        }
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{nameof(MyLanguage)}");
        builder.AppendLine($"{nameof(this.ID)}: {this.ID}");
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

    public TranslationFile GetFlavor(string localeId) {
        return this.Flavors.Where(item => item.LocaleId.Equals(localeId, StringComparison.OrdinalIgnoreCase)).First();
    }
}
