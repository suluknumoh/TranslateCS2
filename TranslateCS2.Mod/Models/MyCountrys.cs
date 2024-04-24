using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Colossal.Localization;

using Game.SceneFlow;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;

using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class MyCountrys {
    private static readonly LocalizationManager LocManager = GameManager.instance.localizationManager;
    public static MyCountrys Instance { get; } = new MyCountrys();
    private readonly Dictionary<SystemLanguage, MyCountry> Dict = [];
    private MyCountrys() {
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
                        // nix
                        continue;
                    case SystemLanguage.SerboCroatian:
                        if (culture.EnglishName.StartsWith("Serbian", StringComparison.OrdinalIgnoreCase)
                            || culture.EnglishName.StartsWith("Croatian", StringComparison.OrdinalIgnoreCase)
                        ) {
                            if (this.Dict.TryGetValue(language, out MyCountry? country) && country != null) {
                                country.CultureInfos.Add(culture);
                            } else {
                                country = new MyCountry(language);
                                this.Dict.Add(language, country);
                            }
                        }
                        continue;
                    case SystemLanguage.ChineseSimplified:
                        comparator = "Chinese (Simplified";
                        break;
                    case SystemLanguage.ChineseTraditional:
                        comparator = "Chinese (Traditional";
                        break;
                    default:
                        comparator = language.ToString();
                        break;
                }
                if (culture.EnglishName.StartsWith(comparator, StringComparison.OrdinalIgnoreCase)) {
                    if (this.Dict.TryGetValue(language, out MyCountry? country) && country != null) {
                        country.CultureInfos.Add(culture);
                    } else {
                        country = new MyCountry(language);
                        this.Dict.Add(language, country);
                    }
                }
            }
        }
        foreach (KeyValuePair<SystemLanguage, MyCountry> entry in this.Dict) {
            entry.Value.Init();
        }
    }
    public void ReadFiles() {
        IEnumerable<string> translationFilePaths = Directory.EnumerateFiles(FileSystemHelper.DataFolder, ModConstants.JsonSearchPattern);
        foreach (string translationFilePath in translationFilePaths) {
            try {
                string name = translationFilePath
                    .Replace(FileSystemHelper.DataFolder, String.Empty)
                    .Replace(ModConstants.JsonExtension, String.Empty);
                if (!this.IsReadAble(name)) {
                    continue;
                }
                MyCountry? country = this.GetCountry(name);
                if (country == null) {
                    continue;
                }
                TranslationFile translationFile = new TranslationFile(name, translationFilePath);
                country.Flavors.Add(translationFile);
            } catch (Exception ex) {
                Mod.Logger.LogError(this.GetType(),
                                    ModConstants.FailedToLoad,
                                    [translationFilePath, ex]);
            }
        }
    }
    public MyCountry? GetCountry(string name) {
        foreach (MyCountry country in this.Dict.Values) {
            IEnumerable<CultureInfo> cis = country.CultureInfos.Where(ci => ci.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (cis.Any()) {
                return country;
            }
        }
        return null;
    }
    private bool IsReadAble(string id) {
        return id.Contains("-") && !LocaleHelper.BuiltIn.Contains(id);
    }

    public void ClearOverwritten() {
        foreach (KeyValuePair<SystemLanguage, MyCountry> entry in this.Dict) {
            if (entry.Value.IsBuiltIn) {
                foreach (TranslationFile translationFile in entry.Value.Flavors) {
                    this.TryToClear(translationFile);
                }
            }
        }
    }
    private void TryToClear(TranslationFile translationFile) {
        try {
            LocManager.RemoveSource(translationFile.LocaleId,
                                    translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(this.GetType(),
                                ModConstants.FailedToUnLoad,
                                [ex, translationFile]);
        }
    }

    public void Reload() {
        // TODO:
    }
}
