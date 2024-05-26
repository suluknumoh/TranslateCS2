using System.Collections.Generic;
using System.Collections.ObjectModel;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.Core.Services.LocalizationFiles;
internal class AppLocFileServiceStrategy : ILocFileServiceStrategy<AppLocFile,
                                                                   AppLocFileSource,
                                                                   ObservableCollection<KeyValuePair<string, IAppLocFileEntry>>,
                                                                   IAppLocFileEntry> {
    public IAppLocFileEntry CreateEntryValue(string key, string value) {
        return new AppLocFileEntry(key, value);
    }

    public AppLocFile CreateNewFile(string id,
                                    string nameEnglish,
                                    string name,
                                    AppLocFileSource source) {
        return new AppLocFile(id,
                              nameEnglish,
                              name,
                              source);
    }

    public AppLocFileSource CreateNewSource() {
        return new AppLocFileSource();
    }
}
