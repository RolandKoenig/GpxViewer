using System;
using System.Collections.Generic;
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

        public BoundingBox CachedBoundingBox { get; }

        public GpxViewerLineString(
            ILoadedGpxFileTourInfo tour, ILoadedGpxFileTourSegmentInfo segment, 
            IEnumerable<Point> points)
            : base(points)
        {
            this.Tour = tour;
            this.Segment = segment;

            // ReSharper disable once VirtualMemberCallInConstructor
            this.CachedBoundingBox = this.BoundingBox ?? throw new InvalidOperationException("Unable to construct a bounding box!");
        }
    }
}
