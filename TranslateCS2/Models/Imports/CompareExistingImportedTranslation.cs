using TranslateCS2.Helpers;

namespace TranslateCS2.Models.Imports;
internal class CompareExistingImportedTranslation {
    public string Key { get; }
    public string? TranslationExisting { get; }
    public bool IsTranslationExistingAvailable => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.TranslationExisting);
    public string? TranslationImported { get; }
    public bool IsTranslationImportedAvailable => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.TranslationImported);
    public CompareExistingImportedTranslation(string key, string? translationExisting, string? translationImported) {
        this.Key = key;
        this.TranslationExisting = translationExisting;
        this.TranslationImported = translationImported;
    }
}
