using System;

using TranslateCS2.Models.LocDictionary;

namespace TranslateCS2.Models.Filters;
internal class LocalizationKeyFilter {
    public string Name { get; }
    public string[]? Values { get; }
    public FilterTypes FilterType { get; }
    public LocalizationKeyFilter(string name, string[]? values, FilterTypes filterType) {
        this.Name = name;
        this.Values = values;
        this.FilterType = filterType;
    }
    public bool Matches(LocalizationDictionaryEntry entry) {
        if (this.Values == null) {
            return true;
        }
        foreach (string valueToMatch in this.Values) {
            switch (this.FilterType) {
                case FilterTypes.StartsWith:
                    if (entry.Key.StartsWith(valueToMatch, StringComparison.OrdinalIgnoreCase)) {
                        return true;
                    }
                    break;
                case FilterTypes.Contains:
                    if (entry.Key.Contains(valueToMatch, StringComparison.OrdinalIgnoreCase)) {
                        return true;
                    }
                    break;
            }
        }
        return false;
    }
}
