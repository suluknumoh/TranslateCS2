using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;

using TranslateCS2.Inf;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
public class Locales {
    private readonly IModRuntimeContainer runtimeContainer;
    private IReadOnlyDictionary<string, string> LowerCaseToBuiltIn { get; }
    internal Locales(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
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
            foreach (SystemLanguage language in languages) {
                string? comparator = null;
                switch (language) {
                    case SystemLanguage.Chinese:
                    // i think i cant handle it, because chinese simplified and chinese traditional are present
                    case SystemLanguage.Unknown:
                        // no need to check for unknown
                        continue;
                    case SystemLanguage.SerboCroatian:
                        if (culture.EnglishName.StartsWith(LangConstants.Serbian, StringComparison.OrdinalIgnoreCase)
                            || culture.EnglishName.StartsWith(LangConstants.Croatian, StringComparison.OrdinalIgnoreCase)
                        ) {
                            this.AddToDictionary(systemLanguageCulturesMapping, culture, language);
                            cultures.Remove(culture);
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
                    this.AddToDictionary(systemLanguageCulturesMapping,
                                         culture,
                                         language);
                    cultures.Remove(culture);
                }
            }
        }
        foreach (CultureInfo culture in cultures) {
            // i want to use the existing logic...
            this.AddToDictionary(systemLanguageCulturesMapping,
                                 culture,
                                 SystemLanguage.Unknown);
        }


        return systemLanguageCulturesMapping;
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
    /// <summary>
    ///     never ever change content!!!
    ///     <br/>
    ///     <br/>
    ///     returns a ref to <see cref="LocaleAsset.data"/>s <see cref="LocaleData.indexCounts"/>
    /// </summary>
    internal IReadOnlyDictionary<string, int>? GetIndexCounts(string localeId, bool isBuiltIn) {
        if (this.runtimeContainer.LocManager is null) {
            return null;
        }
        if (isBuiltIn) {
            return this.GetIndexCountsP(localeId);
        }
        return this.GetIndexCountsP(this.runtimeContainer.LocManager.fallbackLocaleId);
    }
    /// <summary>
    ///     never ever change content!!!
    ///     <br/>
    ///     <br/>
    ///     returns a ref to <see cref="LocaleAsset.data"/>s <see cref="LocaleData.indexCounts"/>
    /// </summary>
    private IReadOnlyDictionary<string, int>? GetIndexCountsP(string localeId) {
        IEnumerable<LocaleAsset> localeAssets =
            AssetDatabase.global.GetAssets(default(SearchFilter<LocaleAsset>))
                .Where(item => item.localeId.Equals(localeId, StringComparison.OrdinalIgnoreCase));
        if (localeAssets.Any()) {
            LocaleAsset asset = localeAssets.First();
            LocaleData data = asset.data;
            return data.indexCounts;
        }
        return null;
    }
}
