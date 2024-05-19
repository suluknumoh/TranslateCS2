using System.Collections.Generic;
using System.Collections.ObjectModel;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Core.Models.Localizations;
public class AppLocFile : AMyLocalization<AppLocFileSource,
                                          ObservableCollection<KeyValuePair<string, IAppLocFileEntry>>,
                                          IAppLocFileEntry> {
    public AppLocFile(string id,
                           string nameEnglish,
                           string name,
                           AppLocFileSource source) : base(id,
                                                           nameEnglish,
                                                           name,
                                                           source) { }
}
