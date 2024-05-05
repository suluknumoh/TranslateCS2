using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using TranslateCS2.Inf;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
public class Locales {
    private IReadOnlyDictionary<string, string> LowerCaseToBuiltIn { get; }
    internal Locales(IModRuntimeContainer runtimeContainer) {
        Dictionary<string, string> dictionary = [];
        // has to end with a forward-slash
        string path = $"{runtimeContainer.Paths.StreamingDataPath}Data~/";
        IEnumerable<string> locFiles = Directory.EnumerateFiles(path, ModConstants.LocSearchPattern);
        foreach (string? locFile in locFiles) {
            string locale =
                locFile
                .Replace(path, String.Empty)
                .Replace(ModConstants.LocExtension, String.Empty);
            dictionary.Add(locale.ToLower(), locale);
        }
        this.LowerCaseToBuiltIn = dictionary;
    }
    public string CorrectLocaleId(string localeId) {
        if (this.LowerCaseToBuiltIn.TryGetValue(localeId.ToLower(), out string? ret) && ret != null) {
            return ret;
        }
        IEnumerable<CultureInfo> cis = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (cis.Any()) {
            return cis.First().Name;
        }
        return localeId;
    }
    public bool IsBuiltIn(string localeId) {
        return this.LowerCaseToBuiltIn.ContainsKey(localeId.ToLower());
    }
    public IDictionary<SystemLanguage, IList<CultureInfo>> GetSystemLanguageCulturesMapping(bool doubleCheck) {
        Dictionary<SystemLanguage, IList<CultureInfo>> systemLanguageCulturesMapping = [];
        IEnumerable<SystemLanguage> languages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        CultureInfo[] cultures =
            CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .ToArray();
        if (doubleCheck) {
            cultures =
                cultures
                    .Where(item => LocalesSupported.IsLocaleIdSupported(item.Name))
                    .ToArray();
        }
        foreach (CultureInfo culture in cultures) {
            foreach (SystemLanguage language in languages) {
                string? comparator = null;
                switch (language) {
                    case SystemLanguage.Chinese:
                        // i think i cant handle it, because chinese simplified and chinese traditional are present
                        continue;
                    case SystemLanguage.SerboCroatian:
                        if (culture.EnglishName.StartsWith(LangConstants.Serbian, StringComparison.OrdinalIgnoreCase)
                            || culture.EnglishName.StartsWith(LangConstants.Croatian, StringComparison.OrdinalIgnoreCase)
                        ) {
                            this.AddToDictionary(systemLanguageCulturesMapping, culture, language);
                        }
                        continue;
                    case SystemLanguage.ChineseSimplified:
                        comparator = LangConstants.ChineseSimplified;
                        break;
                    case SystemLanguage.ChineseTraditional:
                        comparator = LangConstants.ChineseTraditional;
                        break;
                    default:
                        comparator = language.ToString();
                        break;
                }
                if (culture.EnglishName.StartsWith(comparator, StringComparison.OrdinalIgnoreCase)) {
                    this.AddToDictionary(systemLanguageCulturesMapping, culture, language);
                }
            }
        }



        return systemLanguageCulturesMapping;
    }
    private void AddToDictionary(IDictionary<SystemLanguage, IList<CultureInfo>> dictionary, CultureInfo culture, SystemLanguage language) {
        if (dictionary.TryGetValue(language, out IList<CultureInfo>? cultureInfos) && cultureInfos != null) {
            cultureInfos.Add(culture);
        } else {
            cultureInfos = [];
            cultureInfos.Add(culture);
            dictionary.Add(language, cultureInfos);
        }
    }
}