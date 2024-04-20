using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Services;
using TranslateCS2.ModBridge;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class TranslationFile : IDictionarySource {
    private static readonly Dictionary<string, int> localeCounters = [];

    static TranslationFile() {
        foreach (string key in TranslationFileService.BuiltIn) {
            localeCounters.Add(key, 0);
        }
    }
    private Dictionary<string, string>? dictionary;
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
    public string Name { get; }
    public string Path { get; }
    public TranslationFile(string name, string path) {
        this.Name = name;
        this.Path = path;
    }
    public void Init() {
        //
        //
        // dont use this.LocaleId, could be extended by a counter
        bool isCountAble = this.InitLocaleId(this.Name, out int counter);
        //
        //
        // dont use this.LocaleId, could be extended by a counter
        SystemLanguageHelperResult systemLanguageResult = this.InitLanguage(this.Name);
        //
        //
        string json = this.ReadJson();
        this.dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        //
        //
        this.InitLocaleName(isCountAble, systemLanguageResult, counter);
    }
    private string ReadJson() {
        return File.ReadAllText(this.Path, Encoding.UTF8);
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
        builder.AppendLine(nameof(TranslationFile));
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.AppendLine($"{nameof(this.Path)}: {this.Path}");
        builder.AppendLine($"{nameof(this.LocaleId)}: {this.LocaleId}");
        builder.AppendLine($"{nameof(this.LocaleName)}: {this.LocaleName}");
        builder.AppendLine($"{nameof(this.Language)}: {this.Language}");
        return builder.ToString();
    }
}
