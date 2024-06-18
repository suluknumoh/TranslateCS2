using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public class LocFileService<E> {
    private readonly LocFileServiceStrategy<E> strategy;

    public LocFileService(LocFileServiceStrategy<E> strategy) {
        this.strategy = strategy;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        DirectoryInfo loc = new DirectoryInfo(this.strategy.LocFileDirectory);
        return
            loc
                .EnumerateFiles(this.strategy.SearchPattern)
                .OrderBy(item => item.Name.Replace(ModConstants.DllExtension, String.Empty));
    }
    public MyLocalization<E> GetLocalizationFile(FileInfo fileInfo) {
        return this.strategy.GetFile(fileInfo);
    }
    public bool ReadContent(MyLocalizationSource<E> source) {
        return this.strategy.ReadContent(source);
    }
}
