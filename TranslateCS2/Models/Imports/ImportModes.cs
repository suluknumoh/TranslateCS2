using System.ComponentModel;

using TranslateCS2.BindingEnums;

namespace TranslateCS2.Models.Imports;
/// <seealso href="https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/"/>
/// <seealso href="https://github.com/brianlagunas/BindingEnumsInWpf"/>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
internal enum ImportModes {
    [Description("NEW:\r\nremoves all existing,\r\nonly read are kept")]
    NEW,
    [Description("LeftJoin:\r\nkeep existing\r\nand\r\nadd missing read")]
    LeftJoin,
    [Description("RightJoin:\r\noverwrite existing with read\r\nand\r\nkeep existing that aren't read")]
    RightJoin
}
