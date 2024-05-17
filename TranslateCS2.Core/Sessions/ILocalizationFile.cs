using System.Collections.Generic;

namespace TranslateCS2.Core.Sessions;
public interface ILocalizationFile : ILocalizationsGetAble<List<ILocalizationEntry>> {
    string FileName { get; }
    short FileHeader { get; }
    string LocaleNameEN { get; }
    string LocaleNameID { get; }
    string LocaleNameLocalized { get; }
    IDictionary<string, int> Indices { get; }
}
