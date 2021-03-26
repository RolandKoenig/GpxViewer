using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;

namespace GpxViewer.Modules.GpxFiles.Interface.Model
{
    public interface ILoadedGpxFileTrackOrRouteInfo
    {
        ILoadedGpxFile File { get; }

        GpxTrack? RawTrackData { get; }

        GpxRoute? RawRouteData { get; }

        GpxTrackOrRoute RawTrackOrRoute { get; }

        TrackOrRouteExtension RawTrackExtensionData { get; }

        IEnumerable<ILoadedGpxFileTrackOrRouteSegmentInfo> Segments { get; }
    }
}
