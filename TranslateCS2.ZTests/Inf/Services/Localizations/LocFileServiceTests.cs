using System.Collections.Generic;
using System.IO;
using System.Linq;

using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;
using TranslateCS2.ZZZTestLib.Services.Localizations;

using Xunit;

namespace TranslateCS2.ZTests.Inf.Services.Localizations;
public class LocFileServiceTests {
    private readonly int expectedLocFileCount = 12;
    private readonly TestLocFileServiceStrategy strategy;
    public LocFileServiceTests() {
        this.strategy = new TestLocFileServiceStrategy();
    }
    [Fact]
    public void GetLocalizationFilesTest() {
        LocFileService<string> locFileService = new LocFileService<string>(this.strategy);
        IEnumerable<FileInfo> locFiles = locFileService.GetLocalizationFiles();
        Assert.Equal(this.expectedLocFileCount, locFiles.Count());
    }
    [Fact]
    public void GetLocalizationFileTest() {
        LocFileService<string> locFileService = new LocFileService<string>(this.strategy);
        IEnumerable<FileInfo> locFileInfos = locFileService.GetLocalizationFiles();
        Assert.Equal(this.expectedLocFileCount, locFileInfos.Count());
        foreach (FileInfo locFileInfo in locFileInfos) {
            MyLocalization<string> locFile = locFileService.GetLocalizationFile(locFileInfo);
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
