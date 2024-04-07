using System.Collections.ObjectModel;

namespace TranslateCS2.Core.Services.Filters;
public interface IFiltersService {
    ObservableCollection<ILocalizationKeyFilter> GetFilters();
}
