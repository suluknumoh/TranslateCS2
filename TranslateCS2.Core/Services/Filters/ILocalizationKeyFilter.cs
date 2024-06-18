using TranslateCS2.Core.Models.Localizations;

namespace TranslateCS2.Core.Services.Filters;
public interface ILocalizationKeyFilter {
    string Name { get; }
    string[]? Values { get; }
    FilterTypes FilterType { get; }
    bool Matches(IAppLocFileEntry entry);
    public static ILocalizationKeyFilter Create(string name,
                                                string[]? values,
                                                FilterTypes filterType) {
        return new LocalizationKeyFilter(name, values, filterType);
    }
}
