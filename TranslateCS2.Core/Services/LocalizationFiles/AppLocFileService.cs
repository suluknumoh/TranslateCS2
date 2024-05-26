using System.Collections.Generic;
using System.IO;
using System.Text;

using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.LocalizationFiles;
internal class AppLocFileService : IAppLocFileService {
    private readonly InstallPathDetector installPathDetector;

    public AppLocFileService(InstallPathDetector installPathDetector) {
        this.installPathDetector = installPathDetector;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        string installPath = this.installPathDetector.DetectInstallPath();
        string locLocation = Path.Combine(installPath, "Cities2_Data", "StreamingAssets", StringConstants.DataTilde);
        DirectoryInfo loc = new DirectoryInfo(locLocation);
        return loc.EnumerateFiles(ModConstants.LocSearchPattern);
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    /// <seealso cref="Colossal.IO.AssetDatabase.LocaleAsset.Load">
    public AppLocFile GetLocalizationFile(FileInfo fileInfo) {
        using Stream stream = File.OpenRead(fileInfo.FullName);
        BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
        uint fileHeader = reader.ReadUInt16();
        string nameEnglish = reader.ReadString();
        string id = reader.ReadString();
        string name = reader.ReadString();
        AppLocFileSource source = new AppLocFileSource();
        AppLocFile localizationFile = new AppLocFile(id,
                                                     nameEnglish,
                                                     name,
                                                     source);
        ReadLocalizationFilesLocalizations(reader, source);
        ReadLocalizationFilesIndices(reader, source);
        return localizationFile;
    }

    private static void ReadLocalizationFilesIndices(BinaryReader reader, AppLocFileSource source) {
        // INFO: currently not needed, so no need to read them
        return;
        int indexCount = reader.ReadInt32();
        for (int i = 0; i < indexCount; i++) {
            string key = reader.ReadString();
            int val = reader.ReadInt32();
            //source.IndexCounts.Add(new KeyValuePair<string, int>(key, val));
        }
    }

    private static void ReadLocalizationFilesLocalizations(BinaryReader reader, AppLocFileSource source) {
        int localizationCount = reader.ReadInt32();
        for (int i = 0; i < localizationCount; i++) {
            string key = reader.ReadString();
            string value = reader.ReadString();
            IAppLocFileEntry entry = new AppLocFileEntry(key, value);
            source.Localizations.Add(new KeyValuePair<string, IAppLocFileEntry>(entry.Key.Key, entry));
        }
    }
}
