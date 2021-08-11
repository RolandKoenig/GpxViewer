using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace GpxViewer.Core.Converters
{
    public class DoubleToGridLengthConverter : IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) { return new GridLength(0, GridUnitType.Star); }
            if (targetType != typeof(GridLength)) { throw new NotSupportedException($"Target type {targetType.FullName} not supported!"); }
            if (value is not double castedValue)
            {
                throw new NotSupportedException($"Source type {value.GetType().FullName} not supported!");
            }

            return new GridLength(castedValue, GridUnitType.Pixel);
        }

        /// <inheritdoc />
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) { return 0.0; }
            if (targetType != typeof(double)) { throw new NotSupportedException($"Target type {targetType.FullName} not supported!"); }
            if (value is not GridLength castedValue)
            {
                throw new NotSupportedException($"Source type {value.GetType().FullName} not supported!");
            }

            if (castedValue.IsAbsolute) { return castedValue.Value; }
            else { return 0.0; }
        }
    }
}
