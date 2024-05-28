using System;
using System.Collections.Generic;
using System.Text;

using Colossal;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Interfaces;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Mod.Containers.Items;
internal class TranslationFile : IIdNameNameEnglishGetAble, IDictionarySource, IEquatable<TranslationFile?> {

    private readonly IModRuntimeContainer runtimeContainer;
    private readonly MyLanguage language;
    private readonly MyLocalization<string> locFile;
    public string Id => this.locFile.Id;
    public string Name { get; }
    public string NameEnglish => this.locFile.NameEnglish;
    public bool IsOK => this.EntryCount > 0;
    public int EntryCount => this.locFile.EntryCount;
    public MyLocalizationSource<string> Source => this.locFile.Source;
    public TranslationFile(IModRuntimeContainer runtimeContainer,
                           MyLanguage language,
                           MyLocalization<string> locFile) {
        this.runtimeContainer = runtimeContainer;
        this.language = language;
        this.locFile = locFile;
        this.Name = this.GetName(locFile.Name);
    }
    private string GetName(string name) {
        if (this.locFile.Source.Localizations is not null) {
            if (this.locFile.Source.Localizations.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string? outLocaleName)
                && outLocaleName is not null
                && !StringHelper.IsNullOrWhiteSpaceOrEmpty(outLocaleName)) {
                return outLocaleName;
            }
        }
        return name;
    }
    public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors,
                                                                 Dictionary<string, int> indexCountsToFill) {
        indexCountsToFill.Clear();
        // has to be added!!! if dictionary is null, fallback/builtin-language is used!!!
        // prefill with this languages index counts
        string localeId = this.language.Id;
        if (!this.language.IsBuiltIn) {
            localeId = this.runtimeContainer.LocManager.FallbackLocaleId;
        }
        this.runtimeContainer.IndexCountsProvider.AddIndexCounts(indexCountsToFill, localeId);
        if (this.locFile.Source.Localizations is null) {
            return [];
        }
        IndexCountHelper.FillIndexCountsAndAutocorrect(this.locFile.Source.Localizations,
                                                       indexCountsToFill);
        return this.locFile.Source.Localizations;
    }
    [MyExcludeFromCoverage]
    public void Unload() {
        //
    }
    [MyExcludeFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationFile);
    }
    [MyExcludeFromCoverage]
    public bool Equals(TranslationFile? other) {
        return other is not null &&
               EqualityComparer<MyLocalization<string>>.Default.Equals(this.locFile, other.locFile) &&
               this.Id == other.Id &&
               this.Name == other.Name &&
               this.NameEnglish == other.NameEnglish;
    }
    [MyExcludeFromCoverage]
    public override int GetHashCode() {
        int hashCode = -1949584393;
        hashCode = (hashCode * -1521134295) + EqualityComparer<MyLocalization<string>>.Default.GetHashCode(this.locFile);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Id);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.NameEnglish);
        return hashCode;
    }
    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.GetType().ToString());
        builder.AppendLine($"{nameof(this.Id)}: {this.Id}");
        builder.AppendLine($"{nameof(this.NameEnglish)}: {this.NameEnglish}");
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.Append($"{nameof(this.locFile)}: {this.locFile}");
        return builder.ToString();
    }
}
