using System;
using System.Collections.Generic;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class ModLocale : IDictionarySource {
    public static IList<string> BuiltIn { get; } = [
        "de-DE",
        "en-US",
        "es-ES",
        "fr-FR",
        "it-IT",
        "ja-JP",
        "ko-KR",
        "pl-PL",
        "pt-BR",
        "ru-RU",
        "zh-HANS",
        "zh_HANT"
    ];
    private static readonly Dictionary<string, int> localeCounters = [];

    static ModLocale() {
        foreach (string key in BuiltIn) {
            localeCounters.Add(key, 0);
        }
    }

    private readonly Dictionary<string, string> dictionary;
    public string? LocaleId { get; private set; }
    public string? LocaleName { get; private set; }
    public SystemLanguage? Language { get; private set; }
    public bool IsOK {
        get {
            if (String.IsNullOrEmpty(this.LocaleId)
                || String.IsNullOrWhiteSpace(this.LocaleId)) {
                return false;
            }
            if (String.IsNullOrEmpty(this.LocaleName)
                || String.IsNullOrWhiteSpace(this.LocaleName)) {
                return false;
            }
            if (this.dictionary == null
                || this.dictionary.Count == 0) {
                return false;
            }
            return this.Language != null;
        }
    }
    private ModLocale(TranslationFile translationFile) {
        string local_localeId = translationFile.Name;
        //
        //
        // dont use this.LocaleId, could be extended by a counter
        bool isCountAble = this.InitLocaleId(local_localeId, out int counter);
        //
        //
        // dont use this.LocaleId, could be extended by a counter
        SystemLanguageHelperResult systemLanguageResult = this.InitLanguage(local_localeId);
        //
        //
        string json = translationFile.ReadJson();
        this.dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        //
        //
        this.InitLocaleName(isCountAble, systemLanguageResult, counter);
    }

    private bool InitLocaleId(string localeIdParameter, out int counter) {
        // dont use this.LocaleId, could be extended by a counter!!!
        bool isCountAble = localeCounters.TryGetValue(localeIdParameter, out counter);
        if (isCountAble) {
            counter++;
            // dont use this.LocaleId, could be extended by a counter!!!
            localeCounters[localeIdParameter] = counter;
            // localeid rules, so we add a counter
            this.LocaleId = localeIdParameter;
            this.LocaleId += counter;
        } else {
            // dont use this.LocaleId, could be extended by a counter!!!
            localeCounters.Add(localeIdParameter, 0);
            this.LocaleId = localeIdParameter;
        }
        return isCountAble;
    }
    private SystemLanguageHelperResult InitLanguage(string localeIdParameter) {
        // dont use this.LocaleId, could be extended by a counter
        SystemLanguageHelperResult systemLanguageResult = SystemLanguageHelper.Get(localeIdParameter);
        this.Language = systemLanguageResult.Language;
        // INFO: there is a localeid to systemlanguage mapping which has to be unique!
        if (SystemLanguageHelper.IsRandomizeLanguage(this.Language)) {
            this.Language = SystemLanguageHelper.Random();
        }
        return systemLanguageResult;
    }
    private void InitLocaleName(bool isCountAble, SystemLanguageHelperResult systemLanguageResult, int counter) {
        // first check if there is a custom localized locale name
        bool got = this.dictionary.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string localeName);
        if (got) {
            this.LocaleName = localeName;
        } else {
            // use this as fallback
            this.LocaleName = systemLanguageResult.Culture?.NativeName;
        }

        if (isCountAble
            && !LocaleNameHelper.IsLocaleNameAvailable(this.LocaleName)) {
            this.LocaleName += $" ({counter})";
        }
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return this.dictionary;
    }

    public void Unload() {
        //
    }

    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(nameof(ModLocale));
        builder.AppendLine($"{nameof(this.LocaleId)}: {this.LocaleId}");
        builder.AppendLine($"{nameof(this.LocaleName)}: {this.LocaleName}");
        builder.AppendLine($"{nameof(this.Language)}: {this.Language}");
        return builder.ToString();
    }

    public static ModLocale Read(TranslationFile translationFile) {
        return new ModLocale(translationFile);
    }
}
