using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationDictionaryEntry : IDataErrorInfo {
    [JsonIgnore]
    List<string> Keys { get; }
    string Key { get; set; }
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
    public bool IsDeleteAble { get; }
    [JsonIgnore]
    bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);
    void AddKey(string keyParameter);
}
