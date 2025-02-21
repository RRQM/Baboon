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
    public class BoolObjectConverter<TValue> : IValueConverter
    {
        /// <summary>
        /// 当为True时的对象内容。
        /// </summary>
        public TValue TrueValue { get; set; }

        /// <summary>
        /// 当为False的对象内容
        /// </summary>
        public TValue FalseValue { get; set; }

        /// <summary>
        /// 当为Null的对象内容
        /// </summary>
        public TValue NullValue { get; set; }

        /// <summary>
        /// 是否支持ConvertBack。默认False
        /// </summary>
        public bool CanConvertBack { get; set; }

        /// <inheritdoc/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return this.NullValue;
            }
            if (value is bool b && b)
            {
                return TrueValue;
            }
            return FalseValue;
        }

        /// <inheritdoc/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!this.CanConvertBack)
            {
                return default;
            }
            return value.Equals(TrueValue);
        }
    }

    /// <summary>
    /// BoolObjectConverter
    /// </summary>
    public class BoolObjectConverter : BoolObjectConverter<object>
    { 
    
    }
}
