using System.Collections.Generic;

namespace TranslateCS2.Models.LocDictionary;
internal class LocalizationDictionaryOccuranceEntry : LocalizationDictionaryEditEntry {
    public List<string> Keys { get; } = [];
    public int Count => this.Keys.Count;
    public LocalizationDictionaryOccuranceEntry(string value) : base(null, value) {
    }
}
