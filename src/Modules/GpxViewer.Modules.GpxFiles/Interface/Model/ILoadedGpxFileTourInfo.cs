using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;

namespace GpxViewer.Modules.GpxFiles.Interface.Model
{
    public interface ILoadedGpxFileTourInfo
    {
        double DistanceKm { get; }

        double ElevationUpMeters { get; }

        double ElevationDownMeters { get; }

        int CountSegments { get; }

        int CountWaypointsWithinSegments { get; }

        ILoadedGpxFile File { get; }

        GpxTrack? RawTrackData { get; }

        GpxRoute? RawRouteData { get; }

        GpxTrackOrRoute RawTrackOrRoute { get; }

        TourExtension RawTourExtensionData { get; }

        IEnumerable<ILoadedGpxFileTourSegmentInfo> Segments { get; }

        IEnumerable<ILoadedGpxFileWaypointInfo> Waypoints { get; }
    }
}
