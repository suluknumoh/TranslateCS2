using System.Collections.Generic;

namespace TranslateCS2.Core.Sessions;
public class LocalizationFile : ILocalizationFile {
    public string FileName { get; }
    public short FileHeader { get; }
    public string LocaleNameEN { get; }
    public string LocaleNameID { get; }
    public string LocaleNameLocalized { get; }
    public List<ILocalizationDictionaryEntry> LocalizationDictionary { get; } = [];
    public List<KeyValuePair<string, int>> Indizes { get; } = [];
    public LocalizationFile(string fileName,
                            short fileHeader,
                            string localeNameEN,
                            string localeNameID,
                            string localeNameLocalized) {
        this.FileName = fileName;
        this.FileHeader = fileHeader;
        this.LocaleNameEN = localeNameEN;
        this.LocaleNameID = localeNameID;
        this.LocaleNameLocalized = localeNameLocalized;
    }
}
