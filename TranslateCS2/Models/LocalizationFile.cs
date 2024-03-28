using System.Collections.Generic;

using TranslateCS2.Models.LocDictionary;

namespace TranslateCS2.Models;
internal class LocalizationFile {
    public string FileName { get; }
    public short FileHeader { get; }
    public string LocaleNameEN { get; }
    public string LocaleNameID { get; }
    public string LocaleNameLocalized { get; }
    public List<LocalizationDictionaryEntry> LocalizationDictionary { get; } = [];
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
