using System.Collections.Generic;

namespace TranslateCS2.Models;
internal class LocalizationFile<T> {
    public string FileName { get; }
    public short FileHeader { get; }
    public string LocaleNameEN { get; }
    public string LocaleNameID { get; }
    public string LocaleNameLocalized { get; }
    public List<T> LocalizationDictionary { get; } = [];
    public List<KeyValuePair<string, int>> Indizes { get; } = [];
    public LocalizationFile(string fileName, short fileHeader, string localeNameEN, string localeNameID, string localeNameLocalized) {
        this.FileName = fileName;
        this.FileHeader = fileHeader;
        this.LocaleNameEN = localeNameEN;
        this.LocaleNameID = localeNameID;
        this.LocaleNameLocalized = localeNameLocalized;
    }
}
