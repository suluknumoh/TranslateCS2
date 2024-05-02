using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Services.Filters;
public interface ILocalizationKeyFilter {
    string Name { get; }
    string[]? Values { get; }
    FilterTypes FilterType { get; }
    bool Matches(ILocalizationDictionaryEntry entry);
    public static ILocalizationKeyFilter Create(string name,
                                                string[]? values,
                                                FilterTypes filterType) {
        return new LocalizationKeyFilter(name, values, filterType);
    }
}
