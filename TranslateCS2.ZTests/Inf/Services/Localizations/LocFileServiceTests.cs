using System.Collections.Generic;
using System.IO;
using System.Linq;

using TranslateCS2.Inf.Interfaces;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.ZZZTestLib.Services.Localizations;

namespace TranslateCS2.ZTests.Inf.Services.Localizations;
public class LocFileServiceTests {
    private readonly IStreamingDatasDataPathProvider streamingDatasDataPathProvider;
    private readonly TestLocFileServiceStrategy strategy;
    public LocFileServiceTests() {
        this.streamingDatasDataPathProvider = new TestStreamingDatasDataPathProvider();
        this.strategy = new TestLocFileServiceStrategy();
    }
    [Fact]
    public void GetLocalizationFilesTest() {
        LocFileService locFileService = new LocFileService(this.streamingDatasDataPathProvider);
        IEnumerable<FileInfo> locFiles = locFileService.GetLocalizationFiles();
        Assert.Equal(12, locFiles.Count());
    }
    [Fact]
    public void GetLocalizationFileTest() {
        LocFileService locFileService = new LocFileService(this.streamingDatasDataPathProvider);
        IEnumerable<FileInfo> locFileInfos = locFileService.GetLocalizationFiles();
        Assert.Equal(12, locFileInfos.Count());
        foreach (FileInfo locFileInfo in locFileInfos) {
            MyLocalization<string> locFile = locFileService.GetLocalizationFile(locFileInfo, this.strategy);
            Assert.NotNull(locFile);
            Assert.NotNull(locFile.Id);
            Assert.NotNull(locFile.Name);
            Assert.NotNull(locFile.NameEnglish);
            Assert.NotNull(locFile.Source);
            Assert.True(locFile.IsOK);
            Assert.NotNull(locFile.Source.Localizations);
            Assert.NotEmpty(locFile.Source.Localizations);
            Assert.NotNull(locFile.Source.IndexCounts);
            Assert.NotEmpty(locFile.Source.IndexCounts);
        }
    }
}
