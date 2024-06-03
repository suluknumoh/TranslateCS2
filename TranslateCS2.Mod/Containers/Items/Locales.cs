using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using TranslateCS2.Inf;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class Locales {
    private readonly IModRuntimeContainer runtimeContainer;
    private IReadOnlyDictionary<string, string> LowerCaseToBuiltIn { get; }
    public Locales(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        Dictionary<string, string> dictionary = [];
        // has to end with a forward-slash
        string path = $"{runtimeContainer.Paths.StreamingDatasDataPath}";
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
        IEnumerable<CultureInfo> cis =
            CultureInfo.GetCultures(CultureTypes.AllCultures)
            .Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (cis.Any()) {
            return cis.First().Name;
        }
        return localeId;
    }
    public bool IsBuiltIn(string localeId) {
        return this.LowerCaseToBuiltIn.ContainsKey(localeId.ToLower());
    }
    public IDictionary<SystemLanguage, IList<CultureInfo>> GetSystemLanguageCulturesMapping() {
        Dictionary<SystemLanguage, IList<CultureInfo>> systemLanguageCulturesMapping = [];
        IEnumerable<SystemLanguage> languages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        List<CultureInfo> cultures =
            CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(item => LocalesSupported.IsLocaleIdSupported(item.Name))
                .ToList();
        for (int i = cultures.Count - 1; i >= 0; i--) {
            CultureInfo culture = cultures[i];
            this.HandleCultureForMapping(systemLanguageCulturesMapping,
                                         languages,
                                         cultures,
                                         culture);
        }
        foreach (CultureInfo culture in cultures) {
            // i want to use the existing logic...
            this.AddToDictionary(systemLanguageCulturesMapping,
                                 culture,
                                 SystemLanguage.Unknown);
        }


        return systemLanguageCulturesMapping;
    }

    private void HandleCultureForMapping(Dictionary<SystemLanguage, IList<CultureInfo>> systemLanguageCulturesMapping,
                                         IEnumerable<SystemLanguage> languages,
                                         List<CultureInfo> cultures,
                                         CultureInfo culture) {
        foreach (SystemLanguage language in languages) {
            string? comparator = this.GetMappingComparator(systemLanguageCulturesMapping,
                                                           cultures,
                                                           culture,
                                                           language);
            if (comparator is null) {
                continue;
            }
            if (culture.EnglishName.StartsWith(comparator,
                                               StringComparison.OrdinalIgnoreCase)) {
                this.AddToDictionary(systemLanguageCulturesMapping,
                                     culture,
                                     language);
                cultures.Remove(culture);
            }
        }
    }

    private string? GetMappingComparator(Dictionary<SystemLanguage, IList<CultureInfo>> systemLanguageCulturesMapping,
                                         List<CultureInfo> cultures,
                                         CultureInfo culture,
                                         SystemLanguage language) {
        string? comparator = null;
        switch (language) {
            case SystemLanguage.Chinese:
            // i think i cant handle it, because chinese simplified and chinese traditional are present
            case SystemLanguage.Unknown:
                // no need to check for unknown
                return null;
            case SystemLanguage.SerboCroatian:
                if (culture.EnglishName.StartsWith(LangConstants.Serbian, StringComparison.OrdinalIgnoreCase)
                    || culture.EnglishName.StartsWith(LangConstants.Croatian, StringComparison.OrdinalIgnoreCase)
                ) {
                    this.AddToDictionary(systemLanguageCulturesMapping, culture, language);
                    cultures.Remove(culture);
                }
                return null;
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
        return comparator;
    }

    private void AddToDictionary(IDictionary<SystemLanguage, IList<CultureInfo>> dictionary,
                                 CultureInfo culture,
                                 SystemLanguage language) {
        if (StringHelper.IsNullOrWhiteSpaceOrEmpty(culture.Name)) {
            return;
        }
        if (dictionary.TryGetValue(language, out IList<CultureInfo>? cultureInfos) && cultureInfos != null) {
            cultureInfos.Add(culture);
        } else {
            cultureInfos = [];
            cultureInfos.Add(culture);
            dictionary.Add(language, cultureInfos);
        }
    }
}
