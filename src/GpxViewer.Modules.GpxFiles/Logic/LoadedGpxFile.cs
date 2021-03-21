using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.Model;
using Mapsui.Geometries;
using Mapsui.Projection;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFile : ILoadedGpxFile
    {
        public GpxFile RawGpxFile { get; }
        public List<IGeometry> RouteAndTrackGeometries { get; }

        IEnumerable<IGeometry> ILoadedGpxFile.RouteAndTrackGeometries => this.RouteAndTrackGeometries;

        public LoadedGpxFile(GpxFile gpxFile)
        {
            this.RawGpxFile = gpxFile;

            this.RouteAndTrackGeometries = new List<IGeometry>();
            foreach(var actRoute in this.RawGpxFile.Routes)
            {
                var actGeneratedGeometry = BuildRouteOrTrackGeometry(actRoute.RoutePoints);
                if(actGeneratedGeometry != null)
                {
                    this.RouteAndTrackGeometries.Add(actGeneratedGeometry);
                }
            }
            foreach(var actTrack in this.RawGpxFile.Tracks)
            {
                foreach(var actSegment in actTrack.Segments)
                {
                    var actGeneratedGeometry = BuildRouteOrTrackGeometry(actSegment.Points);
                    if(actGeneratedGeometry != null)
                    {
                        this.RouteAndTrackGeometries.Add(actGeneratedGeometry);
                    }
                }
            }
        }

        private static IGeometry? BuildRouteOrTrackGeometry(IEnumerable<GpxWaypoint> waypoints)
        {
            var linePoints = new List<Point>();
            foreach (var actPoint in waypoints)
            {
                linePoints.Add(SphericalMercator.FromLonLat(actPoint.Longitude, actPoint.Latitude));
            }
            if (linePoints.Count < 2) { return null; }

            return new LineString(linePoints);
        }
    }
}
