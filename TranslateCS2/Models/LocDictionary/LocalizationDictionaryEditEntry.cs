using System;

namespace TranslateCS2.Models.LocDictionary;
internal class LocalizationDictionaryEditEntry : LocalizationDictionaryEntry {
    public string? ValueMerge { get; set; }
    private string? _Translation;
    public string? Translation {
        get => this._Translation;
        set => this.SetProperty(ref this._Translation, value);
    }
    public bool IsTranslated => !String.IsNullOrEmpty(this.Translation) && !String.IsNullOrWhiteSpace(this.Translation);
    public LocalizationDictionaryEditEntry(string key, string value) : base(key, value) { }
    public LocalizationDictionaryEditEntry(LocalizationDictionaryEditEntry other) : base(other) { }
}
