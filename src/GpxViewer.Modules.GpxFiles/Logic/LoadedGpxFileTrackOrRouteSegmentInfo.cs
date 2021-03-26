using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFileTrackOrRouteSegmentInfo : ILoadedGpxFileTrackOrRouteSegmentInfo
    {
        public List<GpxWaypoint> Points { get; }

        IEnumerable<GpxWaypoint> ILoadedGpxFileTrackOrRouteSegmentInfo.Points => this.Points;

        public LoadedGpxFileTrackOrRouteSegmentInfo(GpxRoute route)
        {
            this.Points = route.RoutePoints;
        }

        public LoadedGpxFileTrackOrRouteSegmentInfo(GpxTrackSegment trackSegment)
        {
            this.Points = trackSegment.Points;
        }
    }
}
