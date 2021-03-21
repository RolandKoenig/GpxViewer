using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Core.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFileTrackInfo : ILoadedGpxFileTrackInfo
    {
        public GpxTrack RawTrackData { get; }

        public TrackExtension RawTrackExtensionData { get; }

        public GpxTrackState State
        {
            get => this.RawTrackExtensionData.State;
            set => this.RawTrackExtensionData.State = value;
        }

        public LoadedGpxFileTrackInfo(GpxTrack rawTrackData)
        {
            this.RawTrackData = rawTrackData;

            rawTrackData.Extensions ??= new GpxExtensions();
            this.RawTrackExtensionData = rawTrackData.Extensions.GetOrCreateExtension<TrackExtension>();
        }
    }
}
