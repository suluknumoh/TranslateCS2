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
/// <summary>
///     mapping between <see cref="SystemLanguage"/> and <see cref="MyLanguage"/>
/// </summary>
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

    /// <summary>
    ///     pre-init all <see cref="MyLanguage"/>s with their <see cref="CultureInfo"/>s
    ///     <br/>
    /// </summary>
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

    /// <summary>
    ///     tries to read files
    ///     <br/>
    ///     and tries to add locales for the read files
    ///     <br/>
    ///     also collects <see cref="Erroneous"/> (see also: <seealso cref="HasErroneous"/>)
    ///     <br/>
    ///     <br/>
    ///     does NOT add sources to added Locales via <see cref="Colossal.Localization.LocalizationManager.AddSource(String, Colossal.IDictionarySource)"/>
    ///     <br/>
    ///     <br/>
    ///     if a source has to be added due to the
    ///     <br/>
    ///     <see cref="Game.Settings.InterfaceSettings.currentLocale"/> is changed
    ///     <br/>
    ///     or a selected Flavor (<see cref="TranslationFile"/>) is changed
    ///     <br/>
    ///     take a look at:
    ///     <br/>
    ///     <see cref="Game.Settings.Setting.onSettingsApplied"/>
    ///     <br/>
    ///     <see cref="Unitys.IntSettingsProvider.SubscribeOnSettingsApplied(Game.Settings.OnSettingsAppliedHandler)"/>
    ///     <br/>
    ///     <see cref="IntSettings.SubscribeOnSettingsApplied(Game.Settings.OnSettingsAppliedHandler)"/>
    ///     <br/>
    ///     <see cref="ModSettings.Apply(Game.Settings.Setting)"/>
    ///     <br/>
    ///     <see cref="ModSettings.OnLocaleChanged"/>
    ///     <br/>
    ///     <see cref="LocManager.FlavorChanged(MyLanguage?, SystemLanguage, String)"/>
    /// </summary>
    public void Init() {
        this.ReadFiles();
        this.AddLocales();
    }

    /// <summary>
    ///     reads all json-files from within this mods data directory
    ///     <br/>
    ///     that are supported
    ///     <br/>
    ///     <see cref="LocalesSupported.IsLocaleIdSupported(String)"/>
    /// </summary>
    private void ReadFiles() {
        LocFileServiceStrategy<string> strategy = new JsonLocFileServiceStrategy(this.runtimeContainer);
        LocFileService<string> locFileService = new LocFileService<string>(strategy);
        IEnumerable<FileInfo> fileInfos = locFileService.GetLocalizationFiles();
        foreach (FileInfo fileInfo in fileInfos) {
            // no need to lower, IsLocaleIdSupported lowers it
            string localeId = fileInfo.Name.Replace(ModConstants.JsonExtension, String.Empty);
            if (LocalesSupported.IsLocaleIdSupported(localeId)) {
                this.TryToReadFile(locFileService, fileInfo);
            }
        }
    }

    /// <summary>
    ///     tries to read the given <paramref name="fileInfo"/> via the given <paramref name="locFileService"/>
    /// </summary>
    /// <param name="locFileService">
    ///     a simple <see cref="LocFileService{E}"/> where <see langword="E"/> is a <see langword="string"/>
    /// </param>
    /// <param name="fileInfo">
    ///     the files to read <see cref="FileInfo"/>
    /// </param>
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

    /// <summary>
    ///     tries to add a Locale
    ///     <br/>
    ///     for each <see cref="MyLanguage"/> that is <see cref="MyLanguage.IsLanguageInitiallyLoadAble"/>
    ///     <br/>
    ///     <seealso cref="LocManager.TryToAddLocale(MyLanguage)"/>
    ///     <br/>
    ///     <seealso cref="Unitys.LocManagerProvider.AddLocale(String, SystemLanguage, String)"/>
    ///     <br/>
    ///     <seealso cref="Colossal.Localization.LocalizationManager.AddLocale(String, SystemLanguage, String)"/>
    /// </summary>
    private void AddLocales() {
        foreach (MyLanguage language in this.LanguageDictionary.Values) {
            if (!language.IsLanguageInitiallyLoadAble()) {
                // dont add built in by default,
                // otherwise the first flavor is automatically applied
                //
                // dont add languages without flavors,
                // otherwise they would be listed within the default interface settings language select
                continue;
            }
            // only add locale!!!
            // the flavor is added/setted somewhere else...
            this.locManager.TryToAddLocale(language);
        }
    }

    /// <summary>
    ///     tries to reload all <see cref="TranslationFile"/>s, that existed at startup
    ///     <br/>
    ///     and collects <see cref="Erroneous"/> (see also: <seealso cref="HasErroneous"/>)
    /// </summary>
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
        foreach (TranslationFile translationFile in language.Flavors) {
            this.ReLoadTranslationFile(locFileService,
                                       language,
                                       translationFile);
        }
    }

    private void ReLoadTranslationFile(LocFileService<string> locFileService,
                                       MyLanguage language,
                                       TranslationFile translationFile) {
        try {
            bool reRead = this.ReReadTranslationFile(locFileService,
                                                     translationFile);
            if (!reRead) {
                return;
            }
            if (this.IsActive(language, translationFile)) {
                this.locManager.ReloadActiveLocale();
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                              [nameof(ReLoad), ex, language, translationFile]);
        }
    }

    private bool IsActive(MyLanguage language, TranslationFile translationFile) {
        return
            language.IsCurrent()
            && translationFile.IsCurrent();
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
        bool parsed = Enum.TryParse(localeId, true, out SystemLanguage systemLanguage);
        if (parsed) {
            return this.GetLanguage(systemLanguage);
        }
        foreach (MyLanguage language in this.LanguageDictionary.Values) {
            if (language.SupportsLocaleId(localeId)) {
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
        // TODO: CultureInfoNames arent logged anymore, cause they are prefiltered; so it has to be changed; unfiltered cultureinfonames have to be obtained...
        try {
            IOrderedEnumerable<KeyValuePair<SystemLanguage, MyLanguage>> ordered =
                this.LanguageDictionary.OrderBy(item => item.Key.ToString());
            StringBuilder markdownBuilder = new StringBuilder();
            foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in ordered) {
                LogMarkdownAndCultureInfoNamesAppendLanguage(markdownBuilder, entry);
            }
            this.runtimeContainer.Logger.LogInfo(this.GetType(),
                                                 "languages markdown:",
                                                 [markdownBuilder]);
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(LogMarkdownAndCultureInfoNames), ex]);
        }
    }

    private static void LogMarkdownAndCultureInfoNamesAppendLanguage(StringBuilder markdownBuilder,
                                                                     KeyValuePair<SystemLanguage, MyLanguage> entry) {
        markdownBuilder.AppendLine($"## {entry.Value.NameEnglish} - {entry.Value.Name}");
        IOrderedEnumerable<CultureInfo> orderedCultures = entry.Value.CultureInfos.OrderBy(item => item.Name);
        markdownBuilder.AppendLine("| Language-(Region)-Code | English Name | Native Name |");
        markdownBuilder.AppendLine($"| {new string('-', 10)} | {new string('-', 10)} | {new string('-', 10)} |");
        foreach (CultureInfo? cultureInfo in orderedCultures) {
            markdownBuilder.AppendLine($"| {cultureInfo.Name} | {cultureInfo.EnglishName} | {cultureInfo.NativeName} |");
        }
        markdownBuilder.AppendLine();
        markdownBuilder.AppendLine();
    }
}
