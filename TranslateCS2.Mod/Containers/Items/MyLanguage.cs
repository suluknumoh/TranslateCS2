using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class MyLanguage : IReLoadAble {
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
    public IList<Flavor> Flavors { get; } = [];
    public int FlavorCount => this.Flavors.Count;
    public SystemLanguage SystemLanguage { get; }
    public bool IsBuiltIn { get; private set; }
    public bool HasFlavorsWithSources => this.Flavors.Any() && this.Flavors.Where(f => f.HasSources).Any();
    public MyLanguage(SystemLanguage systemLanguage,
                      IModRuntimeContainer runtimeContainer,
                      IList<CultureInfo> cultureInfos) {
        this.SystemLanguage = systemLanguage;
        this.runtimeContainer = runtimeContainer;
        this.Init(cultureInfos);
        this.InitFlavors(cultureInfos);
    }

    private void InitFlavors(IList<CultureInfo> cultureInfos) {
        IOrderedEnumerable<CultureInfo> orderedCultureInfos = cultureInfos.OrderBy(ci => ci.Name);
        foreach (CultureInfo cultureInfo in orderedCultureInfos) {
            Flavor flavor = new Flavor(this.runtimeContainer,
                                       this,
                                       cultureInfo.Name,
                                       cultureInfo.NativeName,
                                       cultureInfo.EnglishName);
            this.Flavors.Add(flavor);
        }
    }

    private void Init(IList<CultureInfo> cultureInfos) {
        IEnumerable<CultureInfo> cultureInfosLocal =
            cultureInfos
                .Where(ci => this.runtimeContainer.Locales.IsBuiltIn(ci.Name));
        //
        //
        this.IsBuiltIn = cultureInfosLocal.Any();
        //
        //
        if (!this.IsBuiltIn) {
            cultureInfosLocal = CultureInfoHelper.GetNeutralCultures(cultureInfos);
        }
        if (!cultureInfosLocal.Any()) {
            return;
        }
        cultureInfosLocal = cultureInfosLocal.OrderBy(ci => ci.Name);
        this.InitId(cultureInfosLocal);
        this.InitNames(cultureInfosLocal);
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
            // cannot fall through :(
            //case SystemLanguage.Norwegian:
            //    cultureInfos = cultureInfos = cultureInfos.OrderByDescending(ci => ci.Name);
            //    cultureInfo = cultureInfos.First();
            default:
                if (this.SystemLanguage is SystemLanguage.Norwegian) {
                    cultureInfos = cultureInfos = cultureInfos.OrderByDescending(ci => ci.Name);
                    cultureInfo = cultureInfos.First();
                }
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
        builder.AppendLine($"{nameof(this.HasFlavorsWithSources)}: {this.HasFlavorsWithSources}");
        builder.AppendLine($"{nameof(this.Flavors)}: ({String.Join(", ", this.Flavors)})");
        return builder.ToString();
    }

    public Flavor? GetFlavor(string localeId) {
        IEnumerable<Flavor> matches =
            this.Flavors
                .Where(item => item.Id.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (matches.Any()) {
            return matches.First();
        }
        return null;
    }

    public IEnumerable<DropdownItem<string>> GetFlavorDropDownItems() {
        List<DropdownItem<string>> dropdownItems = [];
        foreach (Flavor flavor in this.Flavors) {
            if (!flavor.HasSources) {
                continue;
            }
            DropdownItem<string> item = flavor.GetDropDownItem();
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
    public bool HasFlavorWithSources(string localeId) {
        return
            this.Flavors
                .Where(item => item.Id.Equals(localeId, StringComparison.OrdinalIgnoreCase))
                .Where(item => item.HasSources)
                .Any();
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
            && this.HasFlavorsWithSources;
    }

    public bool IsCurrent() {
        string currentLocale = this.runtimeContainer.IntSettings.CurrentLocale;
        return this.Id.Equals(currentLocale,
                              StringComparison.OrdinalIgnoreCase);
    }

    public void ReLoad(LocFileService<string> locFileService) {
        foreach (Flavor flavor in this.Flavors) {
            flavor.ReLoad(locFileService);
        }
    }
    public IEnumerable<FlavorSource> GetErroneous() {
        List<FlavorSource> erroneous = [];
        foreach (Flavor flavor in this.Flavors) {
            erroneous.AddRange(flavor.GetErroneous());
        }
        return erroneous;
    }
}
