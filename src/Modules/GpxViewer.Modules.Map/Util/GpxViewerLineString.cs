using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using Mapsui.Geometries;

namespace GpxViewer.Modules.Map.Util
{
    internal class GpxViewerLineString : LineString
    {
        public ILoadedGpxFileTourInfo Tour
        {
            get;
        }

        public ILoadedGpxFileTourSegmentInfo Segment
        {
            get;
        }

        public GpxViewerLineString(
            ILoadedGpxFileTourInfo tour, ILoadedGpxFileTourSegmentInfo segment, 
            IEnumerable<Point> points)
            : base(points)
        {
            this.Tour = tour;
            this.Segment = segment;
        }
    }
}
