using System.Collections.Generic;
using System.IO;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public class LocFileService<E> {
    private readonly LocFileServiceStrategy<E> strategy;

    public LocFileService(LocFileServiceStrategy<E> strategy) {
        this.strategy = strategy;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        DirectoryInfo loc = new DirectoryInfo(this.strategy.LocFileDirectory);
        return loc.EnumerateFiles(this.strategy.SearchPattern);
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    /// <seealso cref="Colossal.IO.AssetDatabase.LocaleAsset.Load">
    public MyLocalization<E> GetLocalizationFile(FileInfo fileInfo) {
        return this.strategy.GetFile(fileInfo);
    }
    public bool ReadContent(MyLocalizationSource<E> source) {
        return this.strategy.ReadContent(source);
    }
}
