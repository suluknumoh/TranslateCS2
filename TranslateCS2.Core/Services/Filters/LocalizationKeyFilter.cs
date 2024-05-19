using System;

using TranslateCS2.Core.Models.Localizations;

namespace TranslateCS2.Core.Services.Filters;
internal class LocalizationKeyFilter : ILocalizationKeyFilter {
    public string Name { get; }
    public string[]? Values { get; }
    public FilterTypes FilterType { get; }
    public LocalizationKeyFilter(string name,
                                  string[]? values,
                                  FilterTypes filterType) {
        this.Name = name;
        this.Values = values;
        this.FilterType = filterType;
    }
    public bool Matches(AppLocFileEntry entry) {
        if (this.Values == null) {
            return true;
        }
        foreach (string valueToMatch in this.Values) {
            switch (this.FilterType) {
                case FilterTypes.StartsWith:
                    if (entry.Key.Key.StartsWith(valueToMatch, StringComparison.OrdinalIgnoreCase)) {
                        return true;
                    }
                    break;
                case FilterTypes.Contains:
                    if (entry.Key.Key.Contains(valueToMatch, StringComparison.OrdinalIgnoreCase)) {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}
