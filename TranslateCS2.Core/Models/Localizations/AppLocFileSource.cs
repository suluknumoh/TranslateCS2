using System.Collections.Generic;
using System.Collections.ObjectModel;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Core.Models.Localizations;
public class AppLocFileSource : IMyLocalizationSource<ObservableCollection<AppLocFileEntry>,
                                                      AppLocFileEntry> {
    public ObservableCollection<AppLocFileEntry> Localizations { get; } = [];

    public IDictionary<string, int> Indices { get; } = new Dictionary<string, int>();
}
