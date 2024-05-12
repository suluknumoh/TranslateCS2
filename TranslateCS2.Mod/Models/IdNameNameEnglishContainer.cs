using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Interfaces;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
public class IdNameNameEnglishContainer : IIdNameNameEnglishGetAble {
    public string ID { get; }
    public string Name { get; }
    public string NameEnglish { get; }
    private IdNameNameEnglishContainer(string id, string name, string nameEnglish) {
        this.ID = id;
        this.Name = name;
        this.NameEnglish = nameEnglish;
    }
    public static IIdNameNameEnglishGetAble Create(Locales locales,
                                                   SystemLanguage systemLanguage,
                                                   IEnumerable<CultureInfo> cultureInfos,
                                                   bool isBuiltIn) {
        CultureInfo cultureInfo = cultureInfos.First();
        string ID = locales.CorrectLocaleId(cultureInfo.Name);
        string? name = null;
        string? nameEnglish = null;
        switch (systemLanguage) {
            case SystemLanguage.Unknown:
                ID = systemLanguage.ToString();
                name = LangConstants.OtherLanguages;
                nameEnglish = LangConstants.OtherLanguages;
                break;
            case SystemLanguage.SerboCroatian:
                ID = systemLanguage.ToString();
                name = String.Join("/", cultureInfos.OrderByDescending(ci => ci.Name).Select(ci => ci.NativeName));
                nameEnglish = String.Join("/", cultureInfos.OrderByDescending(ci => ci.Name).Select(ci => ci.EnglishName));
                break;
            case SystemLanguage.Portuguese:
            case SystemLanguage.ChineseSimplified:
            case SystemLanguage.ChineseTraditional:
                // take care: cultureInfo itself is used!
                name = cultureInfo.NativeName;
                nameEnglish = cultureInfo.EnglishName;
                break;
            default:
                if (isBuiltIn) {
                    // take care: cultureInfo's parent is used!
                    name = cultureInfo.Parent.NativeName;
                    nameEnglish = cultureInfo.Parent.EnglishName;
                } else {
                    // take care: cultureInfo itself is used!
                    name = cultureInfo.NativeName;
                    nameEnglish = cultureInfo.EnglishName;
                }
                break;
        }
        return new IdNameNameEnglishContainer(ID, name, nameEnglish);
    }
}
