using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Prism.Mvvm;

namespace TranslateCS2.Models.LocDictionary;
internal class LocalizationDictionaryEntry : BindableBase {
    [JsonIgnore]
    public List<string> Keys { get; } = [];
    public string Key => this.Keys[0];
    [JsonIgnore]
    public int Count => this.Keys.Count;
    [JsonIgnore]
    public string Value { get; }
    [JsonIgnore]
    public string? ValueMerge { get; set; }
    private string? _Translation;
    public string? Translation {
        get => this._Translation;
        set => this.SetProperty(ref this._Translation, value);
    }
    [JsonIgnore]
    public bool IsTranslated => !String.IsNullOrEmpty(this.Translation) && !String.IsNullOrWhiteSpace(this.Translation);
    public LocalizationDictionaryEntry(string key, string value, string? translation) {
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
    public LocalizationDictionaryEntry(LocalizationDictionaryEntry other) : this(other.Key, other.Value, other.Translation) { }
}
