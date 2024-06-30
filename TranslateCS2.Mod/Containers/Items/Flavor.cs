using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Colossal;

using Game.UI.Widgets;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.Mod.Containers.Items;
// INFO: its about hashcode and equals...
internal class Flavor : IReLoadAble, IDictionarySource {

    private readonly IModRuntimeContainer runtimeContainer;
    private readonly MyLanguage language;
    public IList<FlavorSource> FlavorSources { get; } = [];
    public bool HasSources => this.FlavorSources.Any();
    public string Id { get; }
    public string NameEnglish { get; }
    private readonly string name;
    public string Name => this.GetName();
    public Flavor(IModRuntimeContainer runtimeContainer,
                  MyLanguage language,
                  string id,
                  string name,
                  string nameEnglish) {
        this.runtimeContainer = runtimeContainer;
        this.language = language;
        this.Id = id;
        this.NameEnglish = nameEnglish;
        this.name = name;
    }
    private string GetName() {
        string name = this.name;
        foreach (FlavorSource source in this.FlavorSources) {
            if (source.Source.Localizations is not null) {
                if (source.Source.Localizations.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string? outLocaleName)
                    && outLocaleName is not null
                    && !StringHelper.IsNullOrWhiteSpaceOrEmpty(outLocaleName)) {
                    name = outLocaleName;
                    break;
                }
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
        IDictionary<string, string> merged = this.GetMergedSources();
        if (merged.Count == 0) {
            return [];
        }
        IndexCountHelper.FillIndexCountsAndAutocorrect(merged,
                                                       indexCountsToFill);
        return merged;
    }

    private IDictionary<string, string> GetMergedSources() {
        Dictionary<string, string> merged = [];
        // to have FlavorSourceTypes.THIS the last one
        IOrderedEnumerable<FlavorSource> orderedSources = this.FlavorSources.OrderByDescending(s => s.SourceType);
        foreach (FlavorSource? orderedSource in orderedSources) {
            foreach (KeyValuePair<string, string> localization in orderedSource.Source.Localizations) {
                merged[localization.Key] = localization.Value;
            }
        }
        return merged;
    }

    [MyExcludeFromCoverage]
    public void Unload() {
        //
    }
    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.GetType().ToString());
        builder.AppendLine($"{nameof(this.Id)}: {this.Id}");
        builder.AppendLine($"{nameof(this.NameEnglish)}: {this.NameEnglish}");
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.Append($"{nameof(this.FlavorSources)}: {this.FlavorSources}");
        return builder.ToString();
    }

    public DropdownItem<string> GetDropDownItem() {
        string displayName = StringHelper.CutStringAfterMaxLengthAndAddThreeDots(this.Name,
                                                                                 ModConstants.MaxDisplayNameLength);
        DropdownItem<string> item = new DropdownItem<string>() {
            value = this.Id,
            displayName = displayName
        };
        return item;
    }

    public void ReLoad(LocFileService<string> locFileService) {
        foreach (FlavorSource source in this.FlavorSources) {
            try {
                // reset to false
                source.HasErrors = false;
                // read content returns true, if the content is read
                source.HasErrors = !locFileService.ReadContent(source.Source);
            } catch (Exception ex) {
                this.runtimeContainer.Logger.LogError(this.GetType(),
                                                      LoggingConstants.FailedTo,
                                                      [nameof(ReLoad), ex, this.language, this]);
                source.HasErrors = true;
            }
        }
    }
    public IEnumerable<FlavorSource> GetErroneous() {
        return this.FlavorSources.Where(x => x.HasErrors || !x.IsOk);
    }
}
