using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Colossal.IO.AssetDatabase.Internal;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class MyLanguage {
    private readonly IModRuntimeContainer runtimeContainer;
    public string Id { get; private set; }
    /// <summary>
    ///     the name that occurs within the drop-down
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    ///     primarily used for logging and generating the supported languages table
    /// </summary>
    public string NameEnglish { get; private set; }
    public IList<TranslationFile> Flavors { get; } = [];
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
    /// <summary>
    ///     <see cref="CultureInfo"/>s associated with this language
    /// </summary>
    public List<CultureInfo> CultureInfos { get; } = [];
    public SystemLanguage SystemLanguage { get; }
    public bool IsBuiltIn { get; private set; }
    public bool HasFlavors => this.Flavors.Any();
    public MyLanguage(SystemLanguage systemLanguage,
                      IModRuntimeContainer runtimeContainer,
                      IList<CultureInfo> cultureInfos) {
        this.SystemLanguage = systemLanguage;
        this.runtimeContainer = runtimeContainer;
        this.CultureInfos.AddRange(cultureInfos);
        this.Init();
    }
    private void Init() {
        IEnumerable<CultureInfo> cultureInfos =
            this.CultureInfos
                .Where(ci => this.runtimeContainer.Locales.IsBuiltIn(ci.Name));
        //
        //
        this.IsBuiltIn = cultureInfos.Any();
        //
        //
        if (!this.IsBuiltIn) {
            cultureInfos = CultureInfoHelper.GetNeutralCultures(this.CultureInfos);
        }
        if (!cultureInfos.Any()) {
            return;
        }
        this.InitId(cultureInfos);
        this.InitNames(cultureInfos);
    }

    private void InitId(IEnumerable<CultureInfo> cultureInfos) {
        // for non built in languages, it seems to be better to use systemlanguage to string
        this.Id = this.SystemLanguage.ToString();
        if (this.IsBuiltIn) {
            // built in languages must use the locale id:
            // cause co uses it
            /// <see cref="Colossal.Localization.LocalizationManager.SupportsLocale(String)"/>
            CultureInfo cultureInfo = cultureInfos.First();
            this.Id = this.runtimeContainer.Locales.CorrectLocaleId(cultureInfo.Name);
        }
    }

    private void InitNames(IEnumerable<CultureInfo> cultureInfos) {
        CultureInfo cultureInfo = cultureInfos.First();
        switch (this.SystemLanguage) {
            case SystemLanguage.Unknown:
                this.Name = LangConstants.OtherLanguages;
                this.NameEnglish = LangConstants.OtherLanguages;
                break;
            case SystemLanguage.SerboCroatian:
                this.Name = Join(cultureInfos,
                                 StringConstants.ForwardSlash,
                                 true,
                                 ci => ci.NativeName);
                this.NameEnglish = Join(cultureInfos,
                                        StringConstants.ForwardSlash,
                                        true,
                                        ci => ci.EnglishName);
                break;
            case SystemLanguage.Portuguese:
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.ChineseTraditional:
                // take care: cultureInfo itself is used!
                this.Name = cultureInfo.NativeName;
                this.NameEnglish = cultureInfo.EnglishName;
                break;
            default:
                if (this.IsBuiltIn) {
                    // take care: cultureInfo's parent is used!
                    // built-ins are specific but use the neutral name(s)
                    this.Name = cultureInfo.Parent.NativeName;
                    this.NameEnglish = cultureInfo.Parent.EnglishName;
                } else {
                    // take care: cultureInfo itself is used!
                    // non built-ins already use the neutral culture
                    this.Name = cultureInfo.NativeName;
                    this.NameEnglish = cultureInfo.EnglishName;
                }
                break;
        }
    }

    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{nameof(MyLanguage)}");
        builder.AppendLine($"{nameof(this.Id)}: {this.Id}");
        builder.AppendLine($"{nameof(this.NameEnglish)}: {this.NameEnglish}");
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.AppendLine($"{nameof(this.IsBuiltIn)}: {this.IsBuiltIn}");
        builder.AppendLine($"{nameof(this.HasFlavors)}: {this.HasFlavors}");
        builder.AppendLine($"{nameof(this.CultureInfos)}: ({String.Join(", ", this.CultureInfos)})");
        return builder.ToString();
    }

    public CultureInfo? GetCultureInfo(string localeId) {
        IEnumerable<CultureInfo> matches =
            this.CultureInfos
                .Where(item => item.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (matches.Any()) {
            return matches.First();
        }
        return null;
    }

    public IEnumerable<DropdownItem<string>> GetFlavorDropDownItems() {
        List<DropdownItem<string>> dropdownItems = [];
        foreach (TranslationFile translationFile in this.Flavors) {
            DropdownItem<string> item = translationFile.GetDropDownItem();
            dropdownItems.Add(item);
        }
        return dropdownItems;
    }

    public bool HasFlavor(string localeId) {
        return
            this.Flavors
                .Where(item => item.Id.Equals(localeId, StringComparison.OrdinalIgnoreCase))
                .Any();
    }

    public TranslationFile GetFlavor(string localeId) {
        return
            this.Flavors
                .Where(item => item.Id.Equals(localeId, StringComparison.OrdinalIgnoreCase))
                .First();
    }

    public bool SupportsLocaleId(string localeId) {
        IEnumerable<CultureInfo> cis =
                this
                    .CultureInfos
                        .Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        return cis.Any();
    }
    private static string Join(IEnumerable<CultureInfo> cultureInfos,
                               string separator,
                               bool skipDashed,
                               Func<CultureInfo, string> selector) {
        IEnumerable<CultureInfo> pre = cultureInfos;
        if (skipDashed) {
            pre =
                cultureInfos
                    .Where(ci => !ci.Name.Contains(StringConstants.Dash));
        }
        pre = pre.OrderByDescending(ci => ci.Name);
        return
            String.Join(separator,
                        pre.Select(selector));
    }

    public bool IsLanguageInitiallyLoadAble() {
        return
            !this.IsBuiltIn
            && this.HasFlavors;
    }

    public bool IsCurrent() {
        string currentLocale = this.runtimeContainer.IntSettings.CurrentLocale;
        return this.Id.Equals(currentLocale,
                              StringComparison.OrdinalIgnoreCase);
    }
}
