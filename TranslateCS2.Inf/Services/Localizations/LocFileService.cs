using System.Collections.Generic;
using System.IO;
using System.Text;

using TranslateCS2.Inf.Interfaces;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public class LocFileService {
    private readonly IStreamingDatasDataPathProvider streamingDatasDataPathProvider;

    public LocFileService(IStreamingDatasDataPathProvider streamingDatasDataPathProvider) {
        this.streamingDatasDataPathProvider = streamingDatasDataPathProvider;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        DirectoryInfo loc = new DirectoryInfo(this.streamingDatasDataPathProvider.StreamingDatasDataPath);
        return loc.EnumerateFiles(ModConstants.LocSearchPattern);
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    /// <seealso cref="Colossal.IO.AssetDatabase.LocaleAsset.Load">
    public MyLocalization<E> GetLocalizationFile<E>(FileInfo fileInfo,
                                                    LocFileServiceStrategy<E> strategy) {
        using Stream stream = File.OpenRead(fileInfo.FullName);
        BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
        uint fileHeader = reader.ReadUInt16();
        string nameEnglish = reader.ReadString();
        string id = reader.ReadString();
        string name = reader.ReadString();
        MyLocalizationSource<E> source = strategy.CreateNewSource();
        MyLocalization<E> localizationFile = strategy.CreateNewFile(id,
                                                     nameEnglish,
                                                     name,
                                                     source);
        ReadLocalizationFilesLocalizations<E>(reader,
                                              source,
                                              strategy);
        ReadLocalizationFilesIndices<E>(reader,
                                        source);
        return localizationFile;
    }

    private static void ReadLocalizationFilesIndices<E>(BinaryReader reader,
                                                        MyLocalizationSource<E> source) {
        int indexCount = reader.ReadInt32();
        for (int i = 0; i < indexCount; i++) {
            string key = reader.ReadString();
            int val = reader.ReadInt32();
            source.IndexCounts.Add(new KeyValuePair<string, int>(key, val));
        }
    }

    private static void ReadLocalizationFilesLocalizations<E>(BinaryReader reader,
                                                              MyLocalizationSource<E> source,
                                                              LocFileServiceStrategy<E> strategy) {
        int localizationCount = reader.ReadInt32();
        for (int i = 0; i < localizationCount; i++) {
            string key = reader.ReadString();
            string value = reader.ReadString();
            E entry = strategy.CreateEntryValue(key, value);
            source.Localizations.Add(new KeyValuePair<string, E>(key, entry));
        }
    }
}
