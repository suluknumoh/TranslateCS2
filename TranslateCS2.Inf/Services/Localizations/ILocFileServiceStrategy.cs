using System.Collections.Generic;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public interface ILocFileServiceStrategy<LF, S, L, E> where LF : AMyLocalization<S, L, E>
                                                      where S : IMyLocalizationSource<L, E>
                                                      where L : ICollection<KeyValuePair<string, E>> {
    E CreateEntryValue(string key, string value);
    LF CreateNewFile(string id,
                     string nameEnglish,
                     string name,
                     S source);
    S CreateNewSource();
}
