using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace TranslateCS2.Core.Converters;
public class EnumBooleanConverter : IValueConverter {
    public object Convert(object value,
                          Type targetType,
                          object parameter,
                          CultureInfo culture) {
        if (value is null) {
            return false;
        }
        if (parameter is DiscreteObjectKeyFrame dof) {
            return value.Equals(dof.Value);
        }
        return value.Equals(parameter);
    }

    public object ConvertBack(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture) {
        if (value is null) {
            return Binding.DoNothing;
        }
        if (parameter is DiscreteObjectKeyFrame dof) {
            return (bool) value ? dof.Value : Binding.DoNothing;
        }
        return (bool) value ? parameter : Binding.DoNothing;
    }
}
