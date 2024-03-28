using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using TranslateCS2.Models.Filters;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Models.Sessions;

namespace TranslateCS2.Services;
internal class FiltersService {
    private readonly TranslationSessionManager _translationSessionManager;

    public FiltersService(TranslationSessionManager translationSessionManager) {
        this._translationSessionManager = translationSessionManager;
    }
    public ObservableCollection<LocalizationKeyFilter> GetFilters() {
        ObservableCollection<LocalizationKeyFilter> filters = [];
        // kinda 'defaults'
        filters.Add(new LocalizationKeyFilter("none", null, FilterTypes.StartsWith));
        filters.Add(new LocalizationKeyFilter("descriptions", [".description", "_description"], FilterTypes.Contains));
        filters.Add(new LocalizationKeyFilter("names", [".name", "_name"], FilterTypes.Contains));
        filters.Add(new LocalizationKeyFilter("titles", [".title", "_title"], FilterTypes.Contains));
        if (!this._translationSessionManager.IsAppUseAble) {
            // always return defaults!
            return filters;
        }
        List<string> filterValues = [];
        foreach (LocalizationDictionaryEntry entry in this._translationSessionManager.BaseLocalizationFile.LocalizationDictionary) {
            if (entry.Key.Contains(".")) {
                string val = entry.Key.Split(".")[0];
                if (filterValues.Contains(val)) {
                    continue;
                }
                filterValues.Add(val);
            }
        }
        IOrderedEnumerable<string> orderedFitlerValues = filterValues.Order();
        foreach (string? filterValue in orderedFitlerValues) {
            filters.Add(new LocalizationKeyFilter(filterValue, [filterValue], FilterTypes.StartsWith));
        }
        return filters;
    }
}
