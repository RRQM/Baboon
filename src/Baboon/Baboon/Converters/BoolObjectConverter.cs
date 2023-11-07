using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Baboon.Converters
{
    /// <summary>
    /// bool与object的转换。
    /// </summary>
    public class BoolObjectConverter : IValueConverter
    {
        /// <summary>
        /// 当为True时的对象内容。
        /// </summary>
        public object TrueObject { get; set; }

        /// <summary>
        /// 当为False的对象内容
        /// </summary>
        public object FalseObject { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b&&b)
            {
                return TrueObject;
            }
            return FalseObject;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
           return value.Equals(TrueObject);
        }
    }
}
