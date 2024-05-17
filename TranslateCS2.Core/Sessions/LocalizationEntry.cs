using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using Prism.Mvvm;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Properties.I18N;
using TranslateCS2.Inf;
using TranslateCS2.Inf.Models;

namespace TranslateCS2.Core.Sessions;
public class LocalizationEntry : BindableBase, ILocalizationEntry, IEquatable<LocalizationEntry?> {
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
    public bool IsIndexed => IndexCountHelper.IndexMatcher.IsMatch(this.Key);
    [JsonIgnore]
    public string CountKey => IndexCountHelper.GetCountKeyFromKey(this.Key);
    [JsonIgnore]
    public int Index => IndexCountHelper.GetIndexFromKey(this.Key);
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
    public Func<string, string?, IndexCountHelperValidationResult>? IsIndexKeyValid { get; set; }

    public LocalizationEntry(string key,
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
        return this.Equals(obj as LocalizationEntry);
    }

    public bool Equals(LocalizationEntry? other) {
        return other is not null &&
               this._Key == other._Key;
    }

    public override int GetHashCode() {
        return HashCode.Combine(this._Key);
    }

    [JsonConstructor]
    public LocalizationEntry(string key, string? translation) : this(key, null, translation, false) { }
    public LocalizationEntry(string key, string? translation, bool isDeleteAble) : this(key, null, translation, isDeleteAble) { }
    public LocalizationEntry(ILocalizationEntry other) : this(other.Key, other.Value, other.Translation, other.IsDeleteAble) {
        this.ValueMerge = other.ValueMerge;
        this.ValueMergeLanguageCode = other.ValueMergeLanguageCode;
    }

    public static bool operator ==(LocalizationEntry? left, LocalizationEntry? right) {
        return EqualityComparer<LocalizationEntry>.Default.Equals(left, right);
    }

    public static bool operator !=(LocalizationEntry? left, LocalizationEntry? right) {
        return !(left == right);
    }
    public string this[string columnName] {
        get {
            if (!this.IsDeleteAble) {
                return String.Empty;
            }
            switch (columnName) {
                case nameof(this.Key):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Key)) {
                        return I18NEdits.InputWarningKeyEmpty;
                    } else if (this.Key.Contains(' ')) {
                        return I18NEdits.InputWarningSpaces;
                    } else if (this.Key != this.KeyOrigin) {
                        if (this.ExistsKeyInCurrentTranslationSession?.Invoke(this.Key) ?? false) {
                            return I18NEdits.InputWarningKeyDuplicate;
                        } else if (IndexCountHelper.IndexMatcher.IsMatch(this.Key)) {
                            IndexCountHelperValidationResult? result = this.IsIndexKeyValid?.Invoke(this.Key, this.KeyOrigin);
                            if (result != null && !result.IsValid) {
                                return String.Format(I18NEdits.InputWarningKeyIndex, result.NextFreeIndex);
                            }
                        }
                    }
                    break;
                case nameof(this.Translation):
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation)) {
                        return I18NEdits.InputWarningTranslationEmpty;
                    }
                    break;
            }
            return String.Empty;
        }
    }
}
