using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Mod.Containers.Items;
public class TranslationFileSource : IMyLocalizationSource<IDictionary<string, string>, string>, IDictionarySource, IEquatable<TranslationFileSource?> {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly MyLanguage language;
    public IDictionary<string, string> Localizations { get; } = new Dictionary<string, string>();
    public string Path { get; }
    public TranslationFileSource(IModRuntimeContainer runtimeContainer,
                                 MyLanguage language,
                                 string path) {
        this.runtimeContainer = runtimeContainer;
        this.language = language;
        this.Path = path;
    }
    public bool Load() {
        try {
            string json = File.ReadAllText(this.Path, Encoding.UTF8);
            IDictionary<string, string>? temporary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (temporary != null) {
                temporary = DictionaryHelper.GetNonEmpty(temporary);
                this.Localizations.Clear();
                DictionaryHelper.AddAll(temporary, this.Localizations);
                return true;
            }
        } catch (Exception ex) {
            this.runtimeContainer.Logger.LogError(this.GetType(),
                                                  LoggingConstants.FailedTo,
                                                  [nameof(Load), ex, this]);
        }
        return false;
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
        if (this.Localizations is null) {
            return [];
        }
        IndexCountHelper.FillIndexCountsAndAutocorrect(this.Localizations,
                                                       indexCountsToFill);
        return this.Localizations;
    }

    public void Unload() {
        //
    }

    [MyExcludeMethodFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as TranslationFileSource);
    }

    [MyExcludeMethodFromCoverage]
    public bool Equals(TranslationFileSource? other) {
        return other is not null &&
               EqualityComparer<IModRuntimeContainer>.Default.Equals(this.runtimeContainer, other.runtimeContainer) &&
               EqualityComparer<MyLanguage>.Default.Equals(this.language, other.language) &&
               EqualityComparer<IDictionary<string, string>>.Default.Equals(this.Localizations, other.Localizations) &&
               this.Path == other.Path;
    }

    [MyExcludeMethodFromCoverage]
    public override int GetHashCode() {
        int hashCode = 1658491880;
        hashCode = (hashCode * -1521134295) + EqualityComparer<IModRuntimeContainer>.Default.GetHashCode(this.runtimeContainer);
        hashCode = (hashCode * -1521134295) + EqualityComparer<MyLanguage>.Default.GetHashCode(this.language);
        hashCode = (hashCode * -1521134295) + EqualityComparer<IDictionary<string, string>>.Default.GetHashCode(this.Localizations);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Path);
        return hashCode;
    }
}
