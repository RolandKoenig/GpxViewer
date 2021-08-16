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
    internal class LoadedGpxFileTourInfo : ILoadedGpxFileTourInfo
    {
        public LoadedGpxFile File { get; }

        ILoadedGpxFile ILoadedGpxFileTourInfo.File => this.File;

        public GpxTrack? RawTrackData { get; }
        public GpxRoute? RawRouteData { get; }
        public GpxTrackOrRoute RawTrackOrRoute { get; }

        public TourExtension RawTourExtensionData { get; }

        public List<LoadedGpxFileTourSegmentInfo> Segments { get; }

        IEnumerable<ILoadedGpxFileTourSegmentInfo> ILoadedGpxFileTourInfo.Segments => this.Segments;

        public LoadedGpxFileTourInfo(LoadedGpxFile file, GpxRoute rawRouteData)
        {
            this.File = file;
            this.RawRouteData = rawRouteData;
            this.RawTrackOrRoute = rawRouteData;

            rawRouteData.Extensions ??= new GpxExtensions();
            this.RawTourExtensionData = rawRouteData.Extensions.GetOrCreateExtension<RouteExtension>();

            this.Segments = new List<LoadedGpxFileTourSegmentInfo>(1);
            this.Segments.Add(new LoadedGpxFileTourSegmentInfo(rawRouteData));
        }

        public LoadedGpxFileTourInfo(LoadedGpxFile file, GpxTrack rawTrackData)
        {
            this.File = file;
            this.RawTrackData = rawTrackData;
            this.RawTrackOrRoute = rawTrackData;

            rawTrackData.Extensions ??= new GpxExtensions();
            this.RawTourExtensionData = rawTrackData.Extensions.GetOrCreateExtension<TrackExtension>();

            this.Segments = new List<LoadedGpxFileTourSegmentInfo>(rawTrackData.Segments.Count);
            foreach(var actSegment in rawTrackData.Segments)
            {
                this.Segments.Add(new LoadedGpxFileTourSegmentInfo(actSegment));
            }
        }
    }
}
