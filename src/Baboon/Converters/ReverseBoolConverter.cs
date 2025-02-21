using System;
using System.Globalization;
using System.Windows.Data;

namespace Baboon.Converters;

/// <summary>
/// 翻转bool的值
/// </summary>
public class ReverseBoolConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b && b)
        {
            return false;
        }
        return true;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool b && b)
        {
            return false;
        }
        return true;
    }
}
