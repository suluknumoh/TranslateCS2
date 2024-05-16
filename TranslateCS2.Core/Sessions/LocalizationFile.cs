using System.Collections.Generic;

namespace TranslateCS2.Core.Sessions;
public class LocalizationFile : ILocalizationFile {
    public string FileName { get; }
    public short FileHeader { get; }
    public string LocaleNameEN { get; }
    public string LocaleNameID { get; }
    public string LocaleNameLocalized { get; }
    public List<ILocalizationEntry> Localizations { get; } = [];
    public IDictionary<string, int> Indizes { get; } = new Dictionary<string, int>();
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
