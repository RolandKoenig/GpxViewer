using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFileTourSegmentInfo : ILoadedGpxFileTourSegmentInfo
    {
        public List<GpxWaypoint> Points { get; }

        IEnumerable<GpxWaypoint> ILoadedGpxFileTourSegmentInfo.Points => this.Points;

        public LoadedGpxFileTourSegmentInfo(GpxRoute route)
        {
            this.Points = route.RoutePoints;
        }

        public LoadedGpxFileTourSegmentInfo(GpxTrackSegment trackSegment)
        {
            this.Points = trackSegment.Points;
        }
    }
}
