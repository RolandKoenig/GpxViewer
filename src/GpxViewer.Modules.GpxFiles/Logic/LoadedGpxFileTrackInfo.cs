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
        public LoadedGpxFile File { get; }

        ILoadedGpxFile ILoadedGpxFileTrackOrRouteInfo.File => this.File;

        public GpxTrack? RawTrackData { get; }
        public GpxRoute? RawRouteData { get; }
        public GpxTrackOrRoute RawTrackOrRoute { get; }

        public TrackOrRouteExtension RawTrackExtensionData { get; }

        public List<LoadedGpxFileTrackOrRouteSegmentInfo> Segments { get; }

        IEnumerable<ILoadedGpxFileTrackOrRouteSegmentInfo> ILoadedGpxFileTrackOrRouteInfo.Segments => this.Segments;

        public LoadedGpxFileTrackInfo(LoadedGpxFile file, GpxRoute rawRouteData)
        {
            this.File = file;
            this.RawRouteData = rawRouteData;
            this.RawTrackOrRoute = rawRouteData;

            rawRouteData.Extensions ??= new GpxExtensions();
            this.RawTrackExtensionData = rawRouteData.Extensions.GetOrCreateExtension<RouteExtension>();

            this.Segments = new List<LoadedGpxFileTrackOrRouteSegmentInfo>(1);
            this.Segments.Add(new LoadedGpxFileTrackOrRouteSegmentInfo(rawRouteData));
        }

        public LoadedGpxFileTrackInfo(LoadedGpxFile file, GpxTrack rawTrackData)
        {
            this.File = file;
            this.RawTrackData = rawTrackData;
            this.RawTrackOrRoute = rawTrackData;

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
