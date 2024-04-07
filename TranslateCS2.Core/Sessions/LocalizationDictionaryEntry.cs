using System.Collections.Generic;
using System.Text.Json.Serialization;

using Prism.Mvvm;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Sessions;
public class LocalizationDictionaryEntry : BindableBase, ILocalizationDictionaryEntry {
    [JsonIgnore]
    public List<string> Keys { get; } = [];
    public string Key => this.Keys[0];
    [JsonIgnore]
    public int Count => this.Keys.Count;
    [JsonIgnore]
    public string Value { get; }
    [JsonIgnore]
    public string ValueLanguageCode => AppConfigurationManager.LeadingLocLanguageCode;
    [JsonIgnore]
    public string? ValueMerge { get; set; }
    [JsonIgnore]
    public string? ValueMergeLanguageCode { get; set; }
    private string? _Translation;
    public string? Translation {
        get => this._Translation;
        set => this.SetProperty(ref this._Translation, value);
    }
    [JsonIgnore]
    public bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);
    public LocalizationDictionaryEntry(string key,
                                       string value,
                                       string? translation) {
        this.AddKey(key);
        this.Value = value;
        this.Translation = translation;
    }
    public void AddKey(string key) {
        if (key == null) {
            return;
        }
        if (!this.Keys.Contains(key)) {
            this.Keys.Add(key);
        }
    }
    [JsonConstructor]
    public LocalizationDictionaryEntry(string key, string? translation) : this(key, null, translation) { }
    public LocalizationDictionaryEntry(ILocalizationDictionaryEntry other) : this(other.Key, other.Value, other.Translation) { }
}
