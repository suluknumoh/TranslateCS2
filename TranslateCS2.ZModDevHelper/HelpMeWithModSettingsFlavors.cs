using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.UI.Widgets;

using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.ZModDevHelper;
public class HelpMeWithModSettingsFlavors {
    [Fact(Skip = "dont help me each time")]
    //[Fact()]
    public void GenerateLanguageSpecificGetMethods() {
        IEnumerable<SystemLanguage> systemLanguages =
            Enum.GetValues(typeof(SystemLanguage))
                .OfType<SystemLanguage>()
                .OrderBy(item => item.ToString());
        StringBuilder builder = new StringBuilder();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is SystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"public {nameof(DropdownItem<string>)}<{nameof(String).ToLower()}>[] {ModSettings.GetFlavorsLangMethodName(systemLanguage)}() {{");
            builder.AppendLine($"    return {ModSettings.GetFlavorsMethodName()}({nameof(SystemLanguage)}.{systemLanguage});");
            builder.AppendLine($"}}");
        }
        string text = builder.ToString().Replace("\r", "");
        Assert.True(true);
    }
}
