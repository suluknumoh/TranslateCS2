using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class ModLocale : IDictionarySource {
    private static readonly Dictionary<string, int> localeCounters = [];

    static ModLocale() {
        localeCounters.Add("de-DE", 0);
        localeCounters.Add("en-US", 0);
        localeCounters.Add("es-ES", 0);
        localeCounters.Add("fr-FR", 0);
        localeCounters.Add("it-IT", 0);
        localeCounters.Add("ja-JP", 0);
        localeCounters.Add("ko-KR", 0);
        localeCounters.Add("pl-PL", 0);
        localeCounters.Add("pt-BR", 0);
        localeCounters.Add("ru-RU", 0);
        localeCounters.Add("zh-HANS", 0);
        localeCounters.Add("zh_HANT", 0);
    }

    private readonly Dictionary<string, string> dictionary;
    public string? LocaleId { get; }
    public string? LocaleName { get; }
    public SystemLanguage? Language { get; }
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
    private ModLocale(string localeId, string json) {

        // dont use this.LocaleId, could be extended by a counter!!!
        bool isCountAble = localeCounters.TryGetValue(localeId, out int counter);
        if (isCountAble) {
            counter++;
            // dont use this.LocaleId, could be extended by a counter!!!
            localeCounters[localeId] = counter;
            // localeid rules, so we add a counter
            this.LocaleId = localeId;
            this.LocaleId += counter;
        } else {
            // dont use this.LocaleId, could be extended by a counter!!!
            localeCounters.Add(localeId, 0);
            this.LocaleId = localeId;
        }

        // dont use this.LocaleId, could be extended by a counter
        SystemLanguageHelperResult systemLanguageResult = SystemLanguageHelper.Get(localeId);
        this.Language = systemLanguageResult.Language;
        // INFO: there is a localeid to systemlanguage mapping which has to be unique!
        if (SystemLanguageHelper.IsRandomizeLanguage(this.Language)) {
            this.Language = SystemLanguageHelper.Random();
        }

        this.dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

        // first check if there is a custom localized locale name
        bool got = this.dictionary.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string localeName);
        if (got) {
            this.LocaleName = localeName;
        } else {
            // use this as fallback
            this.LocaleName = systemLanguageResult.Culture?.NativeName;
        }

        if (isCountAble) {
            // TODO: algorithm to check if a locale name already exists; if the new localization already has a different name to an existing ones, there is no need to add a counter
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

    public static ModLocale Read(string path) {
        string[] splittedPath = path.Split('/');
        string localeId = splittedPath[splittedPath.Length - 1].Split('.')[0];
        string json = File.ReadAllText(path, Encoding.UTF8);
        return new ModLocale(localeId, json);
    }
}
