using System.Collections.Generic;
using System.IO;
using System.Text;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.LocalizationFiles;
internal class LocalizationFileService : ILocalizationFileService {
    private readonly bool skipWorkAround = AppConfigurationManager.SkipWorkAround;
    private readonly InstallPathDetector installPathDetector;

    public LocalizationFileService(InstallPathDetector installPathDetector) {
        this.installPathDetector = installPathDetector;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        string installPath = this.installPathDetector.DetectInstallPath();
        string locLocation = Path.Combine(installPath, "Cities2_Data", "StreamingAssets", "Data~");
        DirectoryInfo loc = new DirectoryInfo(locLocation);
        return loc.EnumerateFiles(ModConstants.LocSearchPattern);
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    public LocalizationFile GetLocalizationFile(FileInfo fileInfo) {
        using Stream stream = File.OpenRead(fileInfo.FullName);
        BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
        short fileHeader = reader.ReadInt16();
        string localeNameEN = reader.ReadString();
        string localeNameID = reader.ReadString();
        string localeNameLocalized = reader.ReadString();
        LocalizationFile localizationFile = new LocalizationFile(fileInfo.Name,
                                                                 fileHeader,
                                                                 localeNameEN,
                                                                 localeNameID,
                                                                 localeNameLocalized);
        ReadLocalizationFilesLocalizations(reader, localizationFile);
        ReadLocalizationFilesIndices(reader, localizationFile);
        return localizationFile;
    }

    private static void ReadLocalizationFilesIndices(BinaryReader reader, ILocalizationFile localizationFile) {
        int indexCount = reader.ReadInt32();
        for (int i = 0; i < indexCount; i++) {
            string key = reader.ReadString();
            int val = reader.ReadInt32();
            localizationFile.Indices.Add(new KeyValuePair<string, int>(key, val));
        }
    }

    private static void ReadLocalizationFilesLocalizations(BinaryReader reader, ILocalizationFile localizationFile) {
        int localizationCount = reader.ReadInt32();
        for (int i = 0; i < localizationCount; i++) {
            string key = reader.ReadString();
            string value = reader.ReadString();
            ILocalizationEntry originLocalizationKey = new LocalizationEntry(key, value, null, false);
            localizationFile.Localizations.Add(originLocalizationKey);
        }
    }
}
