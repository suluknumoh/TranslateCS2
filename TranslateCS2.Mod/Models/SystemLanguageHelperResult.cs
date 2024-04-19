using System.Globalization;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class SystemLanguageHelperResult {
    public SystemLanguage Language { get; }
    public CultureInfo? Culture { get; }
    public SystemLanguageHelperResult(SystemLanguage language, CultureInfo? culture) {
        this.Language = language;
        this.Culture = culture;
    }
}
