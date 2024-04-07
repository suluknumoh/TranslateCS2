using System.Collections.Generic;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationFile {
    string FileName { get; }
    short FileHeader { get; }
    string LocaleNameEN { get; }
    string LocaleNameID { get; }
    string LocaleNameLocalized { get; }
    List<ILocalizationDictionaryEntry> LocalizationDictionary { get; }
    List<KeyValuePair<string, int>> Indizes { get; }
}
