using TranslateCS2.Helpers;

namespace TranslateCS2.Models.Imports;
internal class CompareExistingImportedTranslations {
    public string Key { get; }
    public string? TranslationExisting { get; }
    public bool IsTranslationExistingAvailable => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.TranslationExisting);
    public string? TranslationImported { get; }
    public bool IsTranslationImportedAvailable => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.TranslationImported);
    public CompareExistingImportedTranslations(string key, string? translationExisting, string? translationImported) {
        this.Key = key;
        this.TranslationExisting = translationExisting;
        this.TranslationImported = translationImported;
    }
}
