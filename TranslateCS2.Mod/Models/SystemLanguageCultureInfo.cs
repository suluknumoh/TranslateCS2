using System.Globalization;
using System.Text;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class SystemLanguageCultureInfo {
    public SystemLanguage Language { get; }
    public CultureInfo? Culture { get; }
    public SystemLanguageCultureInfo(SystemLanguage language, CultureInfo? culture) {
        this.Language = language;
        this.Culture = culture;
    }
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine();
        builder.AppendLine($"{nameof(SystemLanguageCultureInfo)}");
        builder.AppendLine($"{nameof(this.Language)}: {this.Language}");
        builder.AppendLine($"{nameof(this.Culture)}: {this.Culture}");
        return builder.ToString();
    }
}
