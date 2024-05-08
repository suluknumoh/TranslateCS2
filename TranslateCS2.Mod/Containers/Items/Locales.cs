using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Loggers;

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
        IEnumerable<CultureInfo> cis = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
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
                    this.AddToDictionary(systemLanguageCulturesMapping, culture, language);
                    cultures.Remove(culture);
                }
            }
        }
        foreach (CultureInfo culture in cultures) {
            // i want to use the existing logic...
            this.AddToDictionary(systemLanguageCulturesMapping, culture, SystemLanguage.Unknown);
        }


        return systemLanguageCulturesMapping;
    }
    private void AddToDictionary(IDictionary<SystemLanguage, IList<CultureInfo>> dictionary, CultureInfo culture, SystemLanguage language) {
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
    public void LogMarkdownAndCultureInfoNames() {
        try {
            IDictionary<SystemLanguage, IList<CultureInfo>> mapping = this.runtimeContainer.Locales.GetSystemLanguageCulturesMapping();
            IOrderedEnumerable<KeyValuePair<SystemLanguage, IList<CultureInfo>>> ordered = mapping.OrderBy(item => item.Key.ToString());
            StringBuilder cultureInfoBuilder = new StringBuilder();
            StringBuilder markdownBuilder = new StringBuilder();
            foreach (KeyValuePair<SystemLanguage, IList<CultureInfo>> entry in ordered) {
                markdownBuilder.AppendLine($"## {entry.Key}");
                IOrderedEnumerable<CultureInfo> orderedCultures = entry.Value.OrderBy(item => item.Name);
                foreach (CultureInfo? cultureInfo in orderedCultures) {
                    markdownBuilder.AppendLine($"* {cultureInfo.Name.PadRight(10)} - {cultureInfo.EnglishName.PadRight(50)} - {cultureInfo.NativeName}");
                    cultureInfoBuilder.AppendLine($"\"{cultureInfo.Name}\",");
                }
                markdownBuilder.AppendLine();
                markdownBuilder.AppendLine();
            }
            string supportedLanguagesMarkDown = markdownBuilder.ToString().Replace("&", "and")
                //.ReplaceLineEndings("\n")
                ;
            this.runtimeContainer.Logger?.LogInfo(this.GetType(),
                                                  "languages markdown:",
                                                  [supportedLanguagesMarkDown]);
            this.runtimeContainer.Logger?.LogInfo(this.GetType(),
                                                  "culture-infos:",
                                                  [cultureInfoBuilder]);
        } catch (Exception ex) {
            this.runtimeContainer.Logger?.LogError(this.GetType(),
                                                   LoggingConstants.FailedTo,
                                                   [nameof(LogMarkdownAndCultureInfoNames), ex]);
        }
    }
}
