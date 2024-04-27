using System.Text;

using TranslateCS2.ModBridge;

namespace TranslateCS2.ZModDevHelper;
public class HelpMeWithModSettingsFlavors {
    /// <summary>
    ///     generates the codeblocks within the partial class ModSettingsFlavors
    /// </summary>
    [Fact(Skip = "dont help me each time")]
    public void GenerateCodeBlocks() {
        IEnumerable<UnitySystemLanguage> systemLanguages = Enum.GetValues(typeof(UnitySystemLanguage)).OfType<UnitySystemLanguage>();
        StringBuilder builder = new StringBuilder();
        foreach (UnitySystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is UnitySystemLanguage.Unknown or UnitySystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"private string _Flavor{systemLanguage} = InitFlavor(SystemLanguage.{systemLanguage});");
            builder.AppendLine($"[SettingsUIDropdown(typeof(ModSettings), nameof(GetFlavors{systemLanguage}))]");
            builder.AppendLine($"[SettingsUISection(Section, FlavorGroup)]");
            builder.AppendLine($"[SettingsUIHideByCondition(typeof(ModSettings), nameof(IsFlavor{systemLanguage}Hidden))]");
            builder.AppendLine($"[SettingsUIDisableByCondition(typeof(ModSettings), nameof(IsFlavor{systemLanguage}Disabled))]");
            builder.AppendLine($"public string Flavor{systemLanguage} {{");
            builder.AppendLine($"    get => this._Flavor{systemLanguage};");
            builder.AppendLine($"    set => this._Flavor{systemLanguage} = this.GetValueToSet(SystemLanguage.{systemLanguage}, value);");
            builder.AppendLine($"}}");
            builder.AppendLine($"public DropdownItem<string>[] GetFlavors{systemLanguage}() {{");
            builder.AppendLine($"    return GetFlavors(SystemLanguage.{systemLanguage});");
            builder.AppendLine($"}}");
            builder.AppendLine($"public bool IsFlavor{systemLanguage}Hidden() {{");
            builder.AppendLine($"    return IsHidden(SystemLanguage.{systemLanguage});");
            builder.AppendLine($"}}");
            builder.AppendLine($"public bool IsFlavor{systemLanguage}Disabled() {{");
            builder.AppendLine($"    return IsDisabled(SystemLanguage.{systemLanguage});");
            builder.AppendLine($"}}");
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine();
        }
        string text = builder.ToString();
        Assert.True(true);
    }
    [Fact(Skip = "dont help me each time")]
    public void GenerateLables() {
        IEnumerable<UnitySystemLanguage> systemLanguages = Enum.GetValues(typeof(UnitySystemLanguage)).OfType<UnitySystemLanguage>();
        StringBuilder builder = new StringBuilder();
        foreach (UnitySystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is UnitySystemLanguage.Unknown or UnitySystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"{{ this.modSettings.GetOptionLabelLocaleID(nameof(ModSettings.Flavor{systemLanguage})), this.GetLabel(nameof(ModSettings.Flavor{systemLanguage}))}},");
        }
        string text = builder.ToString();
        Assert.True(true);
    }
    [Fact(Skip = "dont help me each time")]
    public void GenerateDescriptions() {
        IEnumerable<UnitySystemLanguage> systemLanguages = Enum.GetValues(typeof(UnitySystemLanguage)).OfType<UnitySystemLanguage>();
        StringBuilder builder = new StringBuilder();
        foreach (UnitySystemLanguage systemLanguage in systemLanguages) {
            if (systemLanguage is UnitySystemLanguage.Unknown or UnitySystemLanguage.Chinese) {
                continue;
            }
            builder.AppendLine($"{{ this.modSettings.GetOptionDescLocaleID(nameof(ModSettings.Flavor{systemLanguage})), this.GetDescription(nameof(ModSettings.Flavor{systemLanguage}))}},");
        }
        string text = builder.ToString();
        Assert.True(true);
    }
}
