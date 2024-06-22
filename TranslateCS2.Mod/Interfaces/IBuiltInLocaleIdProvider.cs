using System.Collections.Generic;

namespace TranslateCS2.Mod.Interfaces;
internal interface IBuiltInLocaleIdProvider {
    IReadOnlyList<string> GetBuiltInLocaleIds();
}
