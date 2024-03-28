using System.ComponentModel;

using TranslateCS2.BindingEnums;

namespace TranslateCS2.Models.Imports;
/// <seealso href="https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/"/>
/// <seealso href="https://github.com/brianlagunas/BindingEnumsInWpf"/>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
internal enum ImportModes {
    [Description("NEW: only imported")]
    New,
    [Description("LeftJoin: existing + imported")]
    LeftJoin,
    [Description("RightJoin: imported + existing")]
    RightJoin
}
