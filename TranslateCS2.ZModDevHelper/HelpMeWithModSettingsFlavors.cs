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
            if (systemLanguage is SystemLanguage.Unknown or SystemLanguage.Chinese) {
                continue;
            }
            //builder.AppendLine($"private string _Flavor{systemLanguage} = InitFlavor(SystemLanguage.{systemLanguage});");
            //builder.AppendLine($"[Include]");
            //builder.AppendLine($"[SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavors{systemLanguage}))]");
            //builder.AppendLine($"[SettingsUISection(Section, FlavorGroup)]");
            //builder.AppendLine($"[SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavor{systemLanguage}Hidden))]");
            //builder.AppendLine($"[SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavor{systemLanguage}Disabled))]");
            //builder.AppendLine($"public string Flavor{systemLanguage} {{");
            //builder.AppendLine($"    get => this._Flavor{systemLanguage};");
            //builder.AppendLine($"    set => this._Flavor{systemLanguage} = this.GetValueToSet(SystemLanguage.{systemLanguage}, value);");
            //builder.AppendLine($"}}");
            builder.AppendLine($"public DropdownItem<string>[] GetFlavors{systemLanguage}() {{");
            builder.AppendLine($"    return GetFlavors(SystemLanguage.{systemLanguage});");
            builder.AppendLine($"}}");
            //builder.AppendLine($"public bool IsFlavor{systemLanguage}Hidden() {{");
            //builder.AppendLine($"    return IsHidden(SystemLanguage.{systemLanguage});");
            //builder.AppendLine($"}}");
            //builder.AppendLine($"public bool IsFlavor{systemLanguage}Disabled() {{");
            //builder.AppendLine($"    return IsDisabled(SystemLanguage.{systemLanguage});");
            //builder.AppendLine($"}}");
        }
        string text = builder.ToString()
            //.ReplaceLineEndings("\n")
            ;
        Assert.True(true);
    }
    [Fact(Skip = "dont help me each time")]
    public void GenerateLables() {
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        StringBuilder builder = new StringBuilder();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is SystemLanguage.Unknown or SystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"this.AddToDictionary(this.modSettings.GetOptionLabelLocaleID(\"Flavor{systemLanguage}\"), this.GetLabel(SystemLanguage.{systemLanguage}), true);");
        }
        string text = builder.ToString()
            //.ReplaceLineEndings("\n")
            ;
        Assert.True(true);
    }
    [Fact(Skip = "dont help me each time")]
    public void GenerateDescriptions() {
        IEnumerable<SystemLanguage> systemLanguages = Enum.GetValues(typeof(SystemLanguage)).OfType<SystemLanguage>();
        StringBuilder builder = new StringBuilder();
        foreach (SystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is SystemLanguage.Unknown or SystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"this.AddToDictionary(this.modSettings.GetOptionDescLocaleID(\"Flavor{systemLanguage}\"), this.GetDescription(SystemLanguage.{systemLanguage}), true);");
        }
        string text = builder.ToString()
            //.ReplaceLineEndings("\n")
            ;
        Assert.True(true);
    }
}
