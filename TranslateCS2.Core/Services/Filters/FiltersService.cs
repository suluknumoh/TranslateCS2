﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;

using TranslateCS2.Core.Configurations.CustomFilters;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Services.Filters;
internal class FiltersService : IFiltersService {
    private readonly ITranslationSessionManager _translationSessionManager;

    public FiltersService(ITranslationSessionManager translationSessionManager) {
        this._translationSessionManager = translationSessionManager;
    }
    public ObservableCollection<ILocalizationKeyFilter> GetFilters() {
        ObservableCollection<ILocalizationKeyFilter> filters = [];
        // kinda 'defaults'
        filters.Add(ILocalizationKeyFilter.Create("none", null, FilterTypes.StartsWith));
        this.AddCustomFilters(filters);
        if (!this._translationSessionManager.IsAppUseAble) {
            // always return defaults!
            return filters;
        }
        List<string> filterValues = [];
        foreach (ILocalizationDictionaryEntry entry in this._translationSessionManager.BaseLocalizationFile.LocalizationDictionary) {
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
            filters.Add(ILocalizationKeyFilter.Create(filterValue, [filterValue], FilterTypes.StartsWith));
        }
        return filters;
    }

    private void AddCustomFilters(ObservableCollection<ILocalizationKeyFilter> filters) {
        FiltersSection? customFilters = ConfigurationManager.GetSection("CustomFilters") as FiltersSection;
        if (customFilters is not null) {
            foreach (FilterConfigElement filter in customFilters.Filters) {
                string filterName = filter.Name;
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(filterName)) {
                    continue;
                }
                string[] filterValues = filter.Values.Split(',');
                if (filterValues.Length == 0) {
                    continue;
                }
                FilterTypes filterType = FilterTypes.Contains.ToString() == filter.CheckMethod ? FilterTypes.Contains : FilterTypes.StartsWith;
                filters.Add(ILocalizationKeyFilter.Create(filterName, filterValues, filterType));
            }
        }
    }
}