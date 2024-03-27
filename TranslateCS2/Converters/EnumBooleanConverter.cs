using System;
using System.Globalization;
using System.Windows.Data;

namespace TranslateCS2.Converters;
internal class EnumBooleanConverter : IValueConverter {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is null) {
            return false;
        }
        return value.Equals(parameter);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
        if (value is null) {
            return Binding.DoNothing;
        }
        return ((bool) value) ? parameter : Binding.DoNothing;
    }
}