using System;

using TranslateCS2.Models.LocDictionary;

namespace TranslateCS2.Models.Filters;
internal class LocalizationKeyFilter {
    public string Name { get; }
    public string[]? Values { get; }
    public FilterType FilterType { get; }
    public LocalizationKeyFilter(string name, string[]? values, FilterType filterType) {
        this.Name = name;
        this.Values = values;
        this.FilterType = filterType;
    }
    public bool Matches(LocalizationDictionaryEditEntry entry) {
        if (this.Values == null) {
            return true;
        }
        foreach (string valueToMatch in this.Values) {
            switch (this.FilterType) {
                case FilterType.StartsWith:
                    if (entry.Key.StartsWith(valueToMatch, StringComparison.OrdinalIgnoreCase)) {
                        return true;
                    }
                    break;
                case FilterType.Contains:
                    if (entry.Key.Contains(valueToMatch, StringComparison.OrdinalIgnoreCase)) {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}
