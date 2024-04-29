using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using Colossal.IO.AssetDatabase.Internal;
using Colossal.Localization;

using Game.SceneFlow;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;

using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class MyLanguages {
    private static readonly LocalizationManager LocManager = GameManager.instance.localizationManager;
    public static MyLanguages Instance { get; } = new MyLanguages();

    private readonly Dictionary<SystemLanguage, MyLanguage> Dict = [];
    public int LanguageCount => this.Dict.Count;
    public int FlavorCountOfAllLanguages {
        get {
            int count = 0;
            if (this.LanguageCount > 0) {
                this.Dict.Values.ForEach(item => count += item.FlavorCount);
            }
            return count;
        }
    }
    public long EntryCountOfAllFlavorsOfAllLanguages {
        get {
            long count = 0;
            if (this.LanguageCount > 0) {
                this.Dict.Values.ForEach(item => count += item.EntryCountOfAllFlavors);
            }
            return count;
        }
    }
    public IList<TranslationFile> Erroneous { get; } = [];
    public bool HasErroneous => this.Erroneous.Count > 0;
    private IDictionary<SystemLanguage, string> FlavorMapping { get; } = new Dictionary<SystemLanguage, string>();
    private MyLanguages() {
        this.Init();
    }

    private void Init() {
        IEnumerable<SystemLanguage> languages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
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
                            this.AddToDictionary(culture, language);
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
                    this.AddToDictionary(culture, language);
                }
            }
        }
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in this.Dict) {
            entry.Value.Init();
        }
    }

    private void AddToDictionary(CultureInfo culture, SystemLanguage language) {
        if (this.Dict.TryGetValue(language, out MyLanguage? country) && country != null) {
            country.CultureInfos.Add(culture);
        } else {
            country = new MyLanguage(language);
            country.CultureInfos.Add(culture);
            this.Dict.Add(language, country);
        }
    }

    public void ReadFiles() {
        IEnumerable<string> translationFilePaths = Directory.EnumerateFiles(FileSystemHelper.DataFolder, ModConstants.JsonSearchPattern);
        foreach (string translationFilePath in translationFilePaths) {
            try {
                string localeIdPre = translationFilePath
                    .Replace(FileSystemHelper.DataFolder, String.Empty)
                    .Replace(ModConstants.JsonExtension, String.Empty);
                string localeId = LocaleHelper.CorrectLocaleId(localeIdPre);
                MyLanguage? language = this.GetLanguage(localeId);
                if (language == null) {
                    continue;
                }
                CultureInfo? cultureInfo = language.GetCultureInfo(localeId);
                if (cultureInfo == null) {
                    Mod.Logger.LogError(this.GetType(),
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
                TranslationFile translationFile = new TranslationFile(localeId, localeName, translationFilePath);
                if (!translationFile.IsOK) {
                    this.Erroneous.Add(translationFile);
                }
                language.Flavors.Add(translationFile);
            } catch (Exception ex) {
                Mod.Logger.LogError(this.GetType(),
                                    LoggingConstants.FailedTo,
                                    [nameof(ReadFiles), translationFilePath, ex]);
            }
        }
    }
    private MyLanguage? GetLanguage(string localeId) {
        foreach (MyLanguage country in this.Dict.Values) {
            IEnumerable<CultureInfo> cis = country.CultureInfos.Where(ci => ci.Name.Equals(localeId, StringComparison.OrdinalIgnoreCase));
            if (cis.Any()) {
                return country;
            }
        }
        return null;
    }
    public MyLanguage? GetLanguage(SystemLanguage systemLanguage) {
        if (this.Dict.TryGetValue(systemLanguage, out MyLanguage? language) && language != null) {
            return language;
        }
        return null;
    }

    public void Load() {
        foreach (MyLanguage language in this.Dict.Values) {
            if (language.IsBuiltIn || !language.HasFlavors) {
                continue;
            }
            try {
                this.TryToAddLocale(language);
                TranslationFile flavor = language.Flavors.First();
                this.TryToAddSource(language, flavor, true);
                this.AddToFlavorMapping(language.SystemLanguage, flavor.LocaleId);
            } catch (Exception ex) {
                Mod.Logger.LogError(this.GetType(),
                                    LoggingConstants.FailedTo,
                                    [nameof(Load), ex, language]);
            }
        }
    }

    public void ReLoad() {
        try {
            this.Erroneous.Clear();
            foreach (MyLanguage language in this.Dict.Values) {
                this.FlavorMapping.TryGetValue(language.SystemLanguage, out string? localeId);
                localeId ??= DropDownItemsHelper.None;
                foreach (TranslationFile translationFile in language.Flavors) {
                    try {
                        this.TryToRemoveSource(language, translationFile);
                        bool reInitialized = translationFile.ReInit();
                        if (!translationFile.IsOK || !reInitialized) {
                            this.Erroneous.Add(translationFile);
                        }
                        if (localeId == translationFile.LocaleId) {
                            this.TryToAddSource(language, translationFile);
                        }
                    } catch (Exception ex) {
                        Mod.Logger.LogError(this.GetType(),
                                            LoggingConstants.FailedTo,
                                            [nameof(ReLoad), ex, localeId, language, translationFile]);
                    }
                }
            }
        } catch (Exception ex) {
            Mod.Logger.LogError(this.GetType(),
                                LoggingConstants.FailedTo,
                                [nameof(ReLoad), ex]);
        }
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{nameof(MyLanguages)}: {this.Dict.Count}");
        foreach (KeyValuePair<SystemLanguage, MyLanguage> entry in this.Dict) {
            builder.AppendLine($"{entry.Key}: {entry.Value}");
        }
        return builder.ToString();
    }

    public void FlavorChanged(MyLanguage? language, SystemLanguage systemLanguage, string localeId) {
        try {
            this.AddToFlavorMapping(systemLanguage, localeId);
            if (language == null) {
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
            Mod.Logger.LogError(this.GetType(),
                                LoggingConstants.FailedTo,
                                [nameof(FlavorChanged), ex, language]);
        }
    }
    private void TryToAddLocale(MyLanguage language) {
        try {
            LocManager.AddLocale(language.ID,
                                 language.SystemLanguage,
                                 language.Name);
        } catch (Exception ex) {
            Mod.Logger.LogError(this.GetType(),
                                LoggingConstants.FailedTo,
                                [nameof(TryToAddLocale), ex, language]);
            LocManager.RemoveLocale(language.ID);
            throw;
        }
    }
    private void TryToAddSource(MyLanguage language, TranslationFile translationFile, bool reThrow = false) {
        try {
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            LocManager.AddSource(language.ID,
                                 translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(this.GetType(),
                                LoggingConstants.FailedTo,
                                [nameof(TryToAddSource), ex, translationFile]);
            this.TryToRemoveSource(language, translationFile);
            if (reThrow) {
                throw;
            }
        }
    }
    private void TryToRemoveSource(MyLanguage language, TranslationFile translationFile) {
        try {
            // has to be languages id, cause the language itself is registered with its own id and the translationfile only refers to it
            LocManager.RemoveSource(language.ID,
                                    translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(this.GetType(),
                                LoggingConstants.FailedTo,
                                [nameof(TryToRemoveSource), ex, translationFile]);
        }
    }
    private void AddToFlavorMapping(SystemLanguage systemLanguage, string localeId) {
        this.FlavorMapping.Remove(systemLanguage);
        this.FlavorMapping.Add(systemLanguage, localeId);
    }
}
