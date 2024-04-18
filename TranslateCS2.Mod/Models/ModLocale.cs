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
    public string LocaleId { get; }
    public string LocaleName { get; }
    public SystemLanguage? Language { get; }
    public bool IsOK {
        get {
            if (System.String.IsNullOrEmpty(this.LocaleId) || System.String.IsNullOrWhiteSpace(this.LocaleId)) {
                return false;
            }
            if (System.String.IsNullOrEmpty(this.LocaleName) || System.String.IsNullOrWhiteSpace(this.LocaleName)) {
                return false;
            }
            if (this.dictionary == null || this.dictionary.Count == 0) {
                return false;
            }
            return this.Language != null;
        }
    }
    private ModLocale(string localeId, string json) {
        this.LocaleId = localeId;
        bool isCountAble = localeCounters.TryGetValue(this.LocaleId, out int counter);
        if (isCountAble) {
            counter++;
            localeCounters[this.LocaleId] = counter;
            // localeid rules, so we add a counter
            this.LocaleId += counter;
        }
        this.dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        bool got = this.dictionary.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string localeName);
        if (got) {
            this.LocaleName = localeName;
            // TODO: algorithm to check if a locale name already exists; if the new localization already has a different name to an existing ones, there is no need to add a counter
            if (isCountAble) {
                this.LocaleName += $" ({counter})";
            }
        }
        this.Language = SystemLanguageHelper.Get(this.LocaleId);
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts) {
        return this.dictionary;
    }

    public void Unload() {
        //
    }

    public static ModLocale Read(string path) {
        string[] splittedPath = path.Split('/');
        string localeId = splittedPath[splittedPath.Length - 1].Split('.')[0];
        string json = File.ReadAllText(path, Encoding.UTF8);
        return new ModLocale(localeId, json);
    }
}
