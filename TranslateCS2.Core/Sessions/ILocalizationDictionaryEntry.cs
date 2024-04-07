using System.Collections.Generic;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationDictionaryEntry {
    List<string> Keys { get; }
    string Key => this.Keys[0];
    int Count => this.Keys.Count;
    string Value { get; }
    string ValueLanguageCode { get; }
    string? ValueMerge { get; set; }
    string? ValueMergeLanguageCode { get; set; }
    string? Translation { get; set; }
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
