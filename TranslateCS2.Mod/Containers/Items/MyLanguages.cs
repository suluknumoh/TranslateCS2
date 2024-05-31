using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Colossal.IO.AssetDatabase.Internal;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Services.Localizations;

using UnityEngine;

namespace TranslateCS2.Mod.Containers.Items;
internal class MyLanguages {
    private static readonly MyLanguages? INSTANCE;
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly LocManager locManager;
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
    public IList<TranslationFile> Erroneous { get; } = [];
    public bool HasErroneous => this.Erroneous.Count > 0;

    public MyLanguages(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.locManager = this.runtimeContainer.LocManager;
        this.InitLanguages();
    }

    private void InitLanguages() {
        IDictionary<SystemLanguage, IList<CultureInfo>> mapping =
            this.runtimeContainer.Locales.GetSystemLanguageCulturesMapping();
        foreach (KeyValuePair<SystemLanguage, IList<CultureInfo>> entry in mapping) {
            MyLanguage language = new MyLanguage(entry.Key,
                                                 this.runtimeContainer,
                                                 entry.Value);
            this.LanguageDictionary.Add(entry.Key, language);
        }
    }

    public void Init() {
        this.ReadFiles();
        this.LoadInitial();
    }

    private void ReadFiles() {
        LocFileServiceStrategy<string> strategy = new JsonLocFileServiceStrategy(this.runtimeContainer);
        LocFileService<string> locFileService = new LocFileService<string>(strategy);
        IEnumerable<FileInfo> fileInfos = locFileService.GetLocalizationFiles();
        foreach (FileInfo fileInfo in fileInfos) {
            // TODO: filter via LocalesSupported???
            if (fileInfo.Name == ModConstants.ModExportKeyValueJsonName) {
                // skip this file, otherwise it produces an error, cause it cant be read
                continue;
            }
            this.TryToReadFile(locFileService, fileInfo);
        }
    }

    private void TryToReadFile(LocFileService<string> locFileService, FileInfo fileInfo) {
        try {
            MyLocalization<string> locFile = locFileService.GetLocalizationFile(fileInfo);
            MyLanguage? language = this.GetLanguage(locFile.Id);
            if (language is null) {
                return;
            }
            TranslationFile translationFile = new TranslationFile(this.runtimeContainer,
                                                                  language,
                                                                  locFile);
            if (!translationFile.IsOK) {
                this.Erroneous.Add(translationFile);
            }
            // yes, add even though the file is not ok
            // this way it can be reloaded without the need to restart the game
            language.Flavors.Add(translationFile);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(TryToReadFile), fileInfo, ex]);
        }
    }

    private void LoadInitial() {
        foreach (MyLanguage language in this.LanguageDictionary.Values) {
            if (!this.IsLanguageInitiallyLoadAble(language)) {
                // dont load built in by default,
                // otherwise the first flavor is automatically applied
                //
                // dont load languages without flavors,
                // otherwise they would be listed within the default interface settings language select
                continue;
            }
            this.locManager.TryToAddLanguageInitially(language);
        }
    }

    private bool IsLanguageInitiallyLoadAble(MyLanguage language) {
        return
            !language.IsBuiltIn
            && language.HasFlavors;
    }

    public void ReLoad() {
        try {
            LocFileServiceStrategy<string> strategy = new JsonLocFileServiceStrategy(this.runtimeContainer);
            LocFileService<string> locFileService = new LocFileService<string>(strategy);
            this.Erroneous.Clear();
            foreach (MyLanguage language in this.LanguageDictionary.Values) {
                this.ReLoadLanguage(locFileService, language);
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(ReLoad), ex]);
        }
    }

    private void ReLoadLanguage(LocFileService<string> locFileService, MyLanguage language) {
        string localeId = this.runtimeContainer.Settings.GetSettedFlavor(language.SystemLanguage);
        foreach (TranslationFile translationFile in language.Flavors) {
            this.ReLoadTranslationFile(locFileService,
                                       language,
                                       localeId,
                                       translationFile);
        }
    }

    private void ReLoadTranslationFile(LocFileService<string> locFileService,
                                       MyLanguage language,
                                       string localeId,
                                       TranslationFile translationFile) {
        try {
            bool reRead = this.ReReadTranslationFile(locFileService,
                                                     translationFile);
            if (!reRead) {
                return;
            }
            if (localeId.Equals(translationFile.Id,
                                StringComparison.OrdinalIgnoreCase)) {
                this.locManager.ReplaceSource(language,
                                              translationFile);
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                              [nameof(ReLoad), ex, localeId, language, translationFile]);
        }
    }

    private bool ReReadTranslationFile(LocFileService<string> locFileService,
                                       TranslationFile translationFile) {
        bool reInitialized = locFileService.ReadContent(translationFile.Source);
        if (reInitialized
            && translationFile.IsOK) {
            return true;
        }
        this.Erroneous.Add(translationFile);
        return false;
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
    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{nameof(MyLanguages)}: {this.LanguageDictionary.Count}");
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in this.LanguageDictionary) {
            builder.AppendLine($"{entry.Key}: {entry.Value}");
        }
        return builder.ToString();
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
