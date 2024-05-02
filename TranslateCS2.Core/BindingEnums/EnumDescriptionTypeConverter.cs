using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

using TranslateCS2.Inf;

namespace TranslateCS2.Core.BindingEnums;
/// <seealso href="https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/"/>
/// <seealso href="https://github.com/brianlagunas/BindingEnumsInWpf"/>
public class EnumDescriptionTypeConverter : EnumConverter {
    public EnumDescriptionTypeConverter(Type type)
        : base(type) {
    }
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (destinationType == typeof(string)) {
            if (value != null) {
                string? vts = value.ToString();
                if (vts != null) {
                    FieldInfo? fi = value.GetType().GetField(vts);
                    if (fi != null) {
                        DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        return attributes.Length > 0 && !StringHelper.IsNullOrWhiteSpaceOrEmpty(attributes[0].Description) ? attributes[0].Description : value.ToString();
                    }
                }
            }
            return String.Empty;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
