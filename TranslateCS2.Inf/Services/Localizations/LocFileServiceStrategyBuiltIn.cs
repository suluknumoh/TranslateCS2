using System.Collections.Generic;
using System.IO;
using System.Text;

using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Inf.Services.Localizations;
public abstract class LocFileServiceStrategyBuiltIn<E> : LocFileServiceStrategy<E> {
    public override string SearchPattern { get; } = ModConstants.LocSearchPattern;
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    /// <seealso cref="Colossal.IO.AssetDatabase.LocaleAsset.Load">
    public override MyLocalization<E> GetFile(FileInfo fileInfo) {
        using Stream stream = File.OpenRead(fileInfo.FullName);
        using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
        uint fileHeader = reader.ReadUInt16();
        string nameEnglish = reader.ReadString();
        string id = reader.ReadString();
        string name = reader.ReadString();
        MyLocalizationSource<E> source = this.CreateNewSource(fileInfo);
        MyLocalization<E> localizationFile = this.CreateNewFile(id,
                                                                nameEnglish,
                                                                name,
                                                                source);
        this.ReadContent(source, stream);
        return localizationFile;
    }
    public override bool ReadContent(MyLocalizationSource<E> source, Stream? stream = null) {
        using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, true);
        this.ReadLocalizationFilesLocalizations(reader,
                                                source);
        this.ReadLocalizationFilesIndices(reader,
                                          source);
        return true;
    }
    private void ReadLocalizationFilesIndices(BinaryReader reader,
                                              MyLocalizationSource<E> source) {
        int indexCount = reader.ReadInt32();
        for (int i = 0; i < indexCount; i++) {
            string key = reader.ReadString();
            int val = reader.ReadInt32();
            source.IndexCounts.Add(new KeyValuePair<string, int>(key, val));
        }
    }
    private void ReadLocalizationFilesLocalizations(BinaryReader reader,
                                                    MyLocalizationSource<E> source) {
        int localizationCount = reader.ReadInt32();
        for (int i = 0; i < localizationCount; i++) {
            string key = reader.ReadString();
            string value = reader.ReadString();
            E entry = this.CreateEntryValue(key, value);
            source.Localizations.Add(new KeyValuePair<string, E>(key, entry));
        }
    }
}
