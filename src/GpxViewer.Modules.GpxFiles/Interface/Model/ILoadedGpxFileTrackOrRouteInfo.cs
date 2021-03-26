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
        GpxTrack? RawTrackData { get; }

        GpxRoute? RawRouteData { get; }

        TrackExtension RawTrackExtensionData { get; }

        GpxTrackState State { get; set; }

        IEnumerable<ILoadedGpxFileTrackOrRouteSegmentInfo> Segments { get; }
    }
}
