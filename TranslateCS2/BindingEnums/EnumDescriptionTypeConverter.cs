﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace TranslateCS2.BindingEnums;
/// <seealso href="https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/"/>
/// <seealso href="https://github.com/brianlagunas/BindingEnumsInWpf"/>
public class EnumDescriptionTypeConverter : EnumConverter {
    public EnumDescriptionTypeConverter(Type type)
        : base(type) {
    }
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) {
        if (destinationType == typeof(string)) {
            if (value != null) {
                FieldInfo? fi = value.GetType().GetField(value.ToString());
                if (fi != null) {
                    DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    return attributes.Length > 0 && !String.IsNullOrEmpty(attributes[0].Description) ? attributes[0].Description : value.ToString();
                }
            }

            return String.Empty;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}
