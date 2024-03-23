namespace TranslateCS2.Models.LocDictionary;
internal class LocalizationDictionaryExportEntry : LocalizationDictionaryEntry {
    public bool IsTranslated { get; }
    public LocalizationDictionaryExportEntry(string key, string value) : base(key, value) {
        this.IsTranslated = true;
    }
    public LocalizationDictionaryExportEntry(LocalizationDictionaryEntry other) : base(other) {
        this.IsTranslated = false;
    }
}
