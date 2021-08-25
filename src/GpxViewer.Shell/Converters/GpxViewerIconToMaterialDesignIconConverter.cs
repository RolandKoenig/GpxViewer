using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using GpxViewer.Core;
using MaterialDesignThemes.Wpf;

namespace GpxViewer.Shell.Converters
{
    internal class GpxViewerIconToMaterialDesignIconConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object value, Type targetType, object? parameter, CultureInfo? culture)
        {
            var gpxViewerIcon = (GpxViewerIconKind) value;
            switch (gpxViewerIcon)
            {
                case GpxViewerIconKind.Folder:
                    return PackIconKind.Folder;
                
                case GpxViewerIconKind.GpxFile:
                    return PackIconKind.File;

                case GpxViewerIconKind.Tour:
                    return PackIconKind.Routes;

                case GpxViewerIconKind.DistanceKm:
                    return PackIconKind.LocationDistance;

                case GpxViewerIconKind.ElevationUpMeters:
                    return PackIconKind.ArrowTop;

                case GpxViewerIconKind.ElevationDownMeters:
                    return PackIconKind.ArrowDown;

                case GpxViewerIconKind.Checked:
                    return PackIconKind.Check;

                default:
                    return null;
            }
        }

        /// <inheritdoc />
        public object? ConvertBack(object value, Type targetType, object? parameter, CultureInfo? culture)
        {
            throw new NotImplementedException();
        }
    }
}
