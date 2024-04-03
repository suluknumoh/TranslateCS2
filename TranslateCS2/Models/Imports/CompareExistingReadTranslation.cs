using TranslateCS2.Helpers;

namespace TranslateCS2.Models.Imports;
internal class CompareExistingReadTranslation {
    public string Key { get; }
    public string? TranslationExisting { get; }
    public bool IsTranslationExistingAvailable => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.TranslationExisting);
    public string? TranslationRead { get; }
    public bool IsTranslationReadAvailable => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.TranslationRead);
    public CompareExistingReadTranslation(string key, string? translationExisting, string? translationImported) {
        this.Key = key;
        this.TranslationExisting = translationExisting;
        this.TranslationRead = translationImported;
    }

    public bool IsEqual() {
        return Equals(this.TranslationExisting, this.TranslationRead);
    }
}
