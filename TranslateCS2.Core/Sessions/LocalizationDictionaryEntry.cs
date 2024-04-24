using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Prism.Mvvm;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Helpers;

namespace TranslateCS2.Core.Sessions;
public class LocalizationDictionaryEntry : BindableBase, ILocalizationDictionaryEntry, IEquatable<LocalizationDictionaryEntry?> {
    [JsonIgnore]
    public List<string> Keys { get; } = [];
    [JsonIgnore]
    public string? KeyOrigin { get; }
    private string _Key;
    public string Key {
        get => this._Key;
        set => this.SetProperty(ref this._Key, value);
    }
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
    public bool IsDeleteAble { get; }
    [JsonIgnore]
    public bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);

    public string Error => String.Empty;

    public Func<string, bool>? ExistsKeyInCurrentTranslationSession { get; set; }

    public LocalizationDictionaryEntry(string key,
                                       string value,
                                       string? translation,
                                       bool isDeleteAble) {
        this.KeyOrigin = key;
        this.Key = key;
        this.AddKey(key);
        this.Value = value;
        this.Translation = translation;
        this.IsDeleteAble = isDeleteAble;
    }
    public void AddKey(string keyParameter) {
        if (keyParameter == null) {
            return;
        }
        string key = keyParameter.Replace("\r", System.String.Empty).Replace("\n", System.String.Empty).Trim();
        if (!this.Keys.Contains(key)) {
            this.Keys.Add(key);
        }
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as LocalizationDictionaryEntry);
    }

    public bool Equals(LocalizationDictionaryEntry? other) {
        return other is not null &&
               this._Key == other._Key;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this._Key);
    }

    [JsonConstructor]
    public LocalizationDictionaryEntry(string key, string? translation) : this(key, null, translation, false) { }
    public LocalizationDictionaryEntry(string key, string? translation, bool isDeleteAble) : this(key, null, translation, isDeleteAble) { }
    public LocalizationDictionaryEntry(ILocalizationDictionaryEntry other) : this(other.Key, other.Value, other.Translation, other.IsDeleteAble) { }

    public static bool operator ==(LocalizationDictionaryEntry? left, LocalizationDictionaryEntry? right) {
        return EqualityComparer<LocalizationDictionaryEntry>.Default.Equals(left, right);
    }

    public static bool operator !=(LocalizationDictionaryEntry? left, LocalizationDictionaryEntry? right) {
        return !(left == right);
    }
    public string this[string columnName] {
        get {
            if (!this.IsDeleteAble) {
                return String.Empty;
            }
            switch (columnName) {
                case nameof(this.Key):
                    // TODO:
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Key)) {
                        return "Key must not be empty!";
                    } else if (this.Key != this.KeyOrigin) {
                        if (this.ExistsKeyInCurrentTranslationSession(this.Key)) {
                            return "Key already exists!";
                        }
                    }
                    break;
                case nameof(this.Translation):
                    // TODO:
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation)) {
                        return "Translation must not be empty!";
                    }
                    break;
            }
            return String.Empty;
        }
    }
}
