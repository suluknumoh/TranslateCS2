using System.Collections.Generic;
using System.Collections.ObjectModel;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Core.Models.Localizations;
public class AppLocFileSource : IMyLocalizationSource<ObservableCollection<KeyValuePair<string, IAppLocFileEntry>>,
                                                      IAppLocFileEntry> {
    public ObservableCollection<KeyValuePair<string, IAppLocFileEntry>> Localizations { get; } = [];

    public ICollection<KeyValuePair<string, int>> IndexCounts { get; } = [];

    public IAppLocFileEntry CreateEntryValue(string key, string value) {
        return new AppLocFileEntry(key, value);
    }
}
