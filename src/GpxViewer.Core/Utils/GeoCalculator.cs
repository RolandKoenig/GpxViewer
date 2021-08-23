using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;

namespace GpxViewer.Core.Util
{
    public static class GeoCalculator
    {
        /// <summary>
        /// Calculates the distance between two geographic coordinates.
        /// </summary>
        public static double CalculateDistanceMeters(GpxWaypoint point1, GpxWaypoint point2)
        {
            // Method implementation from
            // https://stackoverflow.com/questions/60700865/find-distance-between-2-coordinates-in-net-core

            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
