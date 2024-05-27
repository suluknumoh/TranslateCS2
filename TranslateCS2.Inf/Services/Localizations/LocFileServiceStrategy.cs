using System.IO;

using TranslateCS2.Inf.Interfaces;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public abstract class LocFileServiceStrategy<E> : ILocFileDirectoryProvider {
    public abstract string LocFileDirectory { get; }
    public abstract string SearchPattern { get; }
    protected abstract E CreateEntryValue(string key, string value);
    protected MyLocalization<E> CreateNewFile(string id,
                                              string nameEnglish,
                                              string name,
                                              MyLocalizationSource<E> source) {
        return new MyLocalization<E>(id,
                                     nameEnglish,
                                     name,
                                     source);
    }
    protected MyLocalizationSource<E> CreateNewSource(FileInfo fileInfo) {
        return new MyLocalizationSource<E>(fileInfo);
    }
    public abstract MyLocalization<E> GetFile(FileInfo fileInfo);
    public abstract bool ReadContent(MyLocalizationSource<E> source, Stream? streamParamter = null);
}
