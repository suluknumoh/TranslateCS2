using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace TranslateCS2.ZModDevHelper;
public class HelpMeWithModSettingsFlavors {
    /// <summary>
    ///     generates the codeblocks within the partial class ModSettingsFlavors
    /// </summary>
    [Fact(Skip = "dont help me each time")]
    public void GenerateCodeBlocks() {
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        StringBuilder builder = new StringBuilder();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is SystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"public DropdownItem<string>[] GetFlavors{systemLanguage}() {{");
            builder.AppendLine($"    return GetFlavors(SystemLanguage.{systemLanguage});");
            builder.AppendLine($"}}");
        }
        string text = builder.ToString()
            //.ReplaceLineEndings("\n")
            ;
        Assert.True(true);
    }
}
