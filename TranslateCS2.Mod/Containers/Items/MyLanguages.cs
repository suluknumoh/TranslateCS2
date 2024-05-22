using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Colossal.IO.AssetDatabase.Internal;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
public class MyLanguages {
    private static readonly MyLanguages? INSTANCE;
    private readonly IModRuntimeContainer runtimeContainer;
    public Dictionary<SystemLanguage, MyLanguage> LanguageDictionary { get; } = [];
    public int LanguageCount => this.LanguageDictionary.Count;
    public int FlavorCountOfAllLanguages {
        get {
            int count = 0;
            if (this.LanguageCount > 0) {
                this.LanguageDictionary.Values.ForEach(item => count += item.FlavorCount);
            }
            return count;
        }
    }
    public long EntryCountOfAllFlavorsOfAllLanguages {
        get {
            long count = 0;
            if (this.LanguageCount > 0) {
                this.LanguageDictionary.Values.ForEach(item => count += item.EntryCountOfAllFlavors);
            }
            return count;
        }
    }
    internal IList<TranslationFile> Erroneous { get; } = [];
    public bool HasErroneous => this.Erroneous.Count > 0;
    private IDictionary<SystemLanguage, string> FlavorMapping { get; } = new Dictionary<SystemLanguage, string>();
    internal MyLanguages(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.Init();
    }

    private void Init() {
        IDictionary<SystemLanguage, IList<CultureInfo>> mapping =
            this.runtimeContainer.Locales.GetSystemLanguageCulturesMapping();
        foreach (KeyValuePair<SystemLanguage, IList<CultureInfo>> entry in mapping) {
            MyLanguage language = new MyLanguage(entry.Key,
                                                 this.runtimeContainer,
                                                 entry.Value);
            this.LanguageDictionary.Add(entry.Key, language);
        }
    }

    public void ReadFiles() {
        IEnumerable<string> translationFilePaths =
            Directory
                .EnumerateFiles(this.runtimeContainer.Paths.ModsDataPathSpecific, ModConstants.JsonSearchPattern)
                .OrderBy(this.runtimeContainer.Paths.ExtractLocaleIdFromPath);
        foreach (string translationFilePath in translationFilePaths) {
            try {
                string localeIdPre = this.runtimeContainer.Paths.ExtractLocaleIdFromPath(translationFilePath);
                string localeId = this.runtimeContainer.Locales.CorrectLocaleId(localeIdPre);
                MyLanguage? language = this.GetLanguage(localeId);
                if (language is null) {
                    continue;
                }
                CultureInfo? cultureInfo = language.GetCultureInfo(localeId);
                if (cultureInfo is null) {
                    this.runtimeContainer.Logger.LogError(this.GetType(),
                                                          LoggingConstants.NoCultureInfo,
                                                          [translationFilePath, localeId, language]);
                    continue;
                }
                string localeName = cultureInfo.NativeName;
                if (language.SystemLanguage == SystemLanguage.SerboCroatian) {
                    if (cultureInfo.EnglishName.Contains(LangConstants.Latin)) {
                        localeName += $" ({LangConstants.Latin})";
                    } else if (cultureInfo.EnglishName.Contains(LangConstants.Cyrillic)) {
                        localeName += $" ({LangConstants.Cyrillic})";
                    }
                }
                TranslationFileSource source = new TranslationFileSource(this.runtimeContainer,
                                                                         language,
                                                                         translationFilePath);
                source.Load();
                TranslationFile translationFile = new TranslationFile(localeId,
                                                                      cultureInfo.EnglishName,
                                                                      localeName,
                                                                      source);
                if (!translationFile.IsOK) {
                    this.Erroneous.Add(translationFile);
                }
                // yes, add even though the file is not ok
                // this way it can be reloaded without the need to restart the game
                language.Flavors.Add(translationFile);
            } catch (Exception ex) {
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(ReadFiles), translationFilePath, ex]);
            }
        }
    }
    /// <summary>
    ///     gets a <see cref="MyLanguage"/> via a correct <paramref name="localeId"/>
    ///     <br/>
    ///     <br/>
    /// </summary>
    /// <param name="localeId">
    ///     has to be a correct locale id
    /// </param>
    /// <returns>
    ///     <see cref="MyLanguage"/>
    /// </returns>
    public MyLanguage? GetLanguage(string localeId) {
        foreach (MyLanguage language in this.LanguageDictionary.Values) {
            IEnumerable<CultureInfo> cis =
                language
                    .CultureInfos
                        .Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
            if (cis.Any()) {
                return language;
            }
        }
        return null;
    }
    public MyLanguage? GetLanguage(SystemLanguage systemLanguage) {
        if (this.LanguageDictionary.TryGetValue(systemLanguage, out MyLanguage? language) && language != null) {
            return language;
        }
        return null;
    }

    public void Load() {
        foreach (MyLanguage language in this.LanguageDictionary.Values) {
            if (language.IsBuiltIn
                || !language.HasFlavors) {
                // dont load built in by default,
                // otherwise the first flavor is automatically applied
                //
                // dont load languages without flavors,
                // otherwise they would be listed within the default interface settings language select
                continue;
            }
            try {
                this.TryToAddLocale(language);
                TranslationFile flavor = language.Flavors.First();
                this.TryToAddSource(language, flavor, true);
                this.AddToFlavorMapping(language.SystemLanguage, flavor.Id);
            } catch (Exception ex) {
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(Load), ex, language]);
            }
        }
    }

    public void ReLoad() {
        try {
            this.Erroneous.Clear();
            foreach (MyLanguage language in this.LanguageDictionary.Values) {
                this.FlavorMapping.TryGetValue(language.SystemLanguage, out string? localeId);
                localeId ??= DropDownItems.None;
                foreach (TranslationFile translationFile in language.Flavors) {
                    try {
                        this.TryToRemoveSource(language, translationFile);
                        bool reInitialized = translationFile.Source.Load();
                        if (!translationFile.IsOK || !reInitialized) {
                            this.Erroneous.Add(translationFile);
                        }
                        if (localeId.Equals(translationFile.Id, StringComparison.OrdinalIgnoreCase)) {
                            this.TryToAddSource(language, translationFile);
                        }
                    } catch (Exception ex) {
                        this.runtimeContainer.Logger.LogError(this.GetType(),
                                                              LoggingConstants.FailedTo,
                                                              [nameof(ReLoad), ex, localeId, language, translationFile]);
                    }
                }
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(ReLoad), ex]);
        }
    }

    [MyExcludeMethodFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{nameof(MyLanguages)}: {this.LanguageDictionary.Count}");
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in this.LanguageDictionary) {
            builder.AppendLine($"{entry.Key}: {entry.Value}");
        }
        return builder.ToString();
    }

    public void FlavorChanged(MyLanguage? language,
                              SystemLanguage systemLanguage,
                              string localeId) {
        try {
            this.AddToFlavorMapping(systemLanguage, localeId);
            if (language is null) {
                return;
            }
            foreach (TranslationFile flavor in language.Flavors) {
                this.TryToRemoveSource(language, flavor);
            }
            if (language.HasFlavor(localeId)) {
                TranslationFile flavor = language.GetFlavor(localeId);
                this.TryToAddSource(language, flavor);
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(FlavorChanged), ex, language]);
        }
    }
    private void TryToAddLocale(MyLanguage language) {
        try {
            this.runtimeContainer.LocManager.AddLocale(language.Id,
                                                       language.SystemLanguage,
                                                       language.Name);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(TryToAddLocale), ex, language]);
            this.runtimeContainer.LocManager.RemoveLocale(language.Id);
            throw;
        }
    }
    private void TryToAddSource(MyLanguage language,
                                TranslationFile translationFile,
                                bool reThrow = false) {
        try {
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            this.runtimeContainer.LocManager.AddSource(language.Id,
                                                       translationFile.Source);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(TryToAddSource), ex, translationFile]);
            this.TryToRemoveSource(language, translationFile);
            if (reThrow) {
                throw;
            }
        }
    }
    private void TryToRemoveSource(MyLanguage language,
                                   TranslationFile translationFile) {
        try {
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            this.runtimeContainer.LocManager.RemoveSource(language.Id,
                                                          translationFile.Source);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(TryToRemoveSource), ex, translationFile]);
        }
    }
    private void AddToFlavorMapping(SystemLanguage systemLanguage,
                                    string localeId) {
        this.FlavorMapping.Remove(systemLanguage);
        this.FlavorMapping.Add(systemLanguage, localeId);
    }
    public void LogMarkdownAndCultureInfoNames() {
        try {
            IOrderedEnumerable<KeyValuePair<SystemLanguage, MyLanguage>> ordered =
                this.LanguageDictionary.OrderBy(item => item.Key.ToString());
            StringBuilder cultureInfoBuilder = new StringBuilder();
            StringBuilder markdownBuilder = new StringBuilder();
            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in ordered) {
                markdownBuilder.AppendLine($"## {entry.Value.NameEnglish} - {entry.Value.Name}");
                IOrderedEnumerable<CultureInfo> orderedCultures = entry.Value.CultureInfos.OrderBy(item => item.Name);
                markdownBuilder.AppendLine("| Language-(Region)-Code | English Name | Native Name |");
                markdownBuilder.AppendLine($"| {new string('-', 10)} | {new string('-', 10)} | {new string('-', 10)} |");
                foreach (CultureInfo? cultureInfo in orderedCultures) {
                    markdownBuilder.AppendLine($"| {cultureInfo.Name} | {cultureInfo.EnglishName} | {cultureInfo.NativeName} |");
                    cultureInfoBuilder.AppendLine($"\"{cultureInfo.Name}\",");
                }
                markdownBuilder.AppendLine();
                markdownBuilder.AppendLine();
            }
            this.runtimeContainer.Logger.LogInfo(this.GetType(),
                                                 "languages markdown:",
                                                 [markdownBuilder]);
            this.runtimeContainer.Logger.LogInfo(this.GetType(),
                                                 "culture-infos:",
                                                 [cultureInfoBuilder]);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(LogMarkdownAndCultureInfoNames), ex]);
        }
    }
}
