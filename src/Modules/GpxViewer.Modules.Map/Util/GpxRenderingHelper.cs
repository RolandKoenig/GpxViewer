using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using Mapsui.Geometries;
using Mapsui.Projection;

namespace GpxViewer.Modules.Map.Util
{
    internal static class GpxRenderingHelper
    {
        public static IGeometry? GpxWaypointsToMapsuiGeometry(
            this IEnumerable<GpxWaypoint> waypoints,
            ILoadedGpxFileTourInfo tour, ILoadedGpxFileTourSegmentInfo segment)
        {
            var linePoints = new List<Point>();
            foreach (var actPoint in waypoints)
            {
                linePoints.Add(SphericalMercator.FromLonLat(actPoint.Longitude, actPoint.Latitude));
            }
            if (linePoints.Count < 2) { return null; }

            return new GpxViewerLineString(tour, segment, linePoints);
        }
    }
}
