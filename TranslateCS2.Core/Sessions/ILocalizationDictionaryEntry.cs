using System.Collections.Generic;
using System.Text.Json.Serialization;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationDictionaryEntry {
    [JsonIgnore]
    List<string> Keys { get; }
    string Key => this.Keys[0];
    [JsonIgnore]
    int Count => this.Keys.Count;
    [JsonIgnore]
    string Value { get; }
    [JsonIgnore]
    string ValueLanguageCode { get; }
    [JsonIgnore]
    string? ValueMerge { get; set; }
    [JsonIgnore]
    string? ValueMergeLanguageCode { get; set; }
    string? Translation { get; set; }
    [JsonIgnore]
    bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);
    void AddKey(string key) {
        if (key == null) {
            return;
        }
        if (!this.Keys.Contains(key)) {
            this.Keys.Add(key);
        }
    }
}
