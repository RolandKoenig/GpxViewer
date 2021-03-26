using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFileTrackInfo : ILoadedGpxFileTrackOrRouteInfo
    {
        public GpxTrack? RawTrackData { get; }
        public GpxRoute? RawRouteData { get; }

        public TrackExtension RawTrackExtensionData { get; }

        public List<LoadedGpxFileTrackOrRouteSegmentInfo> Segments { get; }

        IEnumerable<ILoadedGpxFileTrackOrRouteSegmentInfo> ILoadedGpxFileTrackOrRouteInfo.Segments => this.Segments;

        public GpxTrackState State
        {
            get => this.RawTrackExtensionData.State;
            set => this.RawTrackExtensionData.State = value;
        }

        public LoadedGpxFileTrackInfo(GpxRoute rawRouteData)
        {
            this.RawRouteData = rawRouteData;

            rawRouteData.Extensions ??= new GpxExtensions();
            this.RawTrackExtensionData = rawRouteData.Extensions.GetOrCreateExtension<TrackExtension>();

            this.Segments = new List<LoadedGpxFileTrackOrRouteSegmentInfo>(1);
            this.Segments.Add(new LoadedGpxFileTrackOrRouteSegmentInfo(rawRouteData));
        }

        public LoadedGpxFileTrackInfo(GpxTrack rawTrackData)
        {
            this.RawTrackData = rawTrackData;

            rawTrackData.Extensions ??= new GpxExtensions();
            this.RawTrackExtensionData = rawTrackData.Extensions.GetOrCreateExtension<TrackExtension>();

            this.Segments = new List<LoadedGpxFileTrackOrRouteSegmentInfo>(rawTrackData.Segments.Count);
            foreach(var actSegment in rawTrackData.Segments)
            {
                this.Segments.Add(new LoadedGpxFileTrackOrRouteSegmentInfo(actSegment));
            }
        }
    }
}
