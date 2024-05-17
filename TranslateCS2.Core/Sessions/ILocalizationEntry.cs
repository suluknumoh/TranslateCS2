using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Keyz;
using TranslateCS2.Inf.Models;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationEntry : IMyKey, IDataErrorInfo {
    [JsonIgnore]
    List<string> Keys { get; }
    [JsonIgnore]
    string? KeyOrigin { get; }
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
    Func<string, bool>? ExistsKeyInCurrentTranslationSession { get; set; }
    Func<string, string?, IndexCountHelperValidationResult>? IsIndexKeyValid { get; set; }
    void AddKey(string keyParameter);
}
