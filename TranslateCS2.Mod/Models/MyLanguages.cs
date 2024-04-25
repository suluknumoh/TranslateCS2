using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;

using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class MyLanguages {
    //private static readonly LocalizationManager LocManager = GameManager.instance.localizationManager;
    public static MyLanguages Instance { get; } = new MyLanguages();
    private readonly Dictionary<SystemLanguage, MyLanguage> Dict = [];
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
                string localeId = translationFilePath
                    .Replace(FileSystemHelper.DataFolder, String.Empty)
                    .Replace(ModConstants.JsonExtension, String.Empty);
                if (!this.IsReadAble(localeId)) {
                    continue;
                }
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
                if (RegExConstants.ContainsNonBasicLatinCharacters.IsMatch(localeName)) {
                    localeName = cultureInfo.EnglishName;
                }
                TranslationFile translationFile = new TranslationFile(localeId, localeName, translationFilePath);
                language.Flavors.Add(translationFile);
            } catch (Exception ex) {
                Mod.Logger.LogError(this.GetType(),
                                    LoggingConstants.FailedTo,
                                    [nameof(ReadFiles), translationFilePath, ex]);
            }
        }
    }
    public MyLanguage? GetLanguage(string name) {
        foreach (MyLanguage country in this.Dict.Values) {
            IEnumerable<CultureInfo> cis = country.CultureInfos.Where(ci => ci.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
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
    private bool IsReadAble(string id) {
        // TODO: is it necessary to check for dash and built-int?
        // TODO: probably check, if cultureinfo exists.
        // TODO:
        return true;
        return id.Contains("-") && !LocaleHelper.BuiltIn.Contains(id);
    }

    public void Load() {
        // TODO: ggf. zusätzliche locales mit sourcen hinzufügen
    }

    public void ReLoad() {
        // TODO: nur versuchen, die json-dateien erneut einzulesen
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
        // TODO: prüfen, ob language die aktuell eingestellte language ist
        // TODO: eigene sourcen removen
        // TODO: source der localeid (Flavor/TranslationFile) hinzufügen
    }
}
