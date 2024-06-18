using System.Collections.Generic;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Keyz;

namespace TranslateCS2.Core.Models.Localizations;
public interface IAppLocFileEntry : IMyKeyProvider {
    HashSet<string> Keys { get; }
    string? KeyOrigin { get; }
    string? Value { get; set; }
    int Count => this.Keys.Count;
    string? ValueMerge { get; set; }
    string? Translation { get; set; }
    bool IsDeleteAble { get; }
    bool IsTranslated => !StringHelper.IsNullOrWhiteSpaceOrEmpty(this.Translation);
    void AddKey(string keyParameter);
    IAppLocFileEntry Clone();
}
