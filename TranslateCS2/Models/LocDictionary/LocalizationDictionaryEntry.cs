using Prism.Mvvm;

namespace TranslateCS2.Models.LocDictionary;
internal class LocalizationDictionaryEntry : BindableBase {
    public string Key { get; }
    public string Value { get; }

    public LocalizationDictionaryEntry(string key, string value) {
        this.Key = key;
        this.Value = value;
    }
    public LocalizationDictionaryEntry(LocalizationDictionaryEntry other) {
        this.Key = other.Key;
        this.Value = other.Value;
    }
}
