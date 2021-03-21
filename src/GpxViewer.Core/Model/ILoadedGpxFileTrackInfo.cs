using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;

namespace GpxViewer.Core.Model
{
    public interface ILoadedGpxFileTrackInfo
    {
        GpxTrack RawTrackData { get; }

        TrackExtension RawTrackExtensionData { get; }

        GpxTrackState State { get; set; }
    }
}
