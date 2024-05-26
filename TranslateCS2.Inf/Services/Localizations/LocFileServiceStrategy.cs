using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public abstract class LocFileServiceStrategy<E> {
    public abstract E CreateEntryValue(string key, string value);

    public MyLocalization<E> CreateNewFile(string id,
                                           string nameEnglish,
                                           string name,
                                           MyLocalizationSource<E> source) {
        return new MyLocalization<E>(id,
                                     nameEnglish,
                                     name,
                                     source);
    }

    public MyLocalizationSource<E> CreateNewSource() {
        return new MyLocalizationSource<E>();
    }
}
