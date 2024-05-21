using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Colossal;

using Newtonsoft.Json;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Mod.Containers.Items;
public class TranslationFileSource : IMyLocalizationSource<IDictionary<string, string>, string>, IDictionarySource {
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
        string localeId = this.language.ID;
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
}
