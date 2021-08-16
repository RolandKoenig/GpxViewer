using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Utils.Mathematics;
using FirLib.Formats.Gpx;
using GpxViewer.Core.GpxExtensions;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using TimeSpan = System.TimeSpan;

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

        public double DistanceKm { get; private set; } = 0.0;

        public double ElevationUpMeters { get; private set; } = 0.0;

        public double ElevationDownMeters { get; private set; } = 0.0;

        public LoadedGpxFileTourInfo(LoadedGpxFile file, GpxRoute rawRouteData)
        {
            this.File = file;
            this.RawRouteData = rawRouteData;
            this.RawTrackOrRoute = rawRouteData;

            rawRouteData.Extensions ??= new GpxExtensions();
            this.RawTourExtensionData = rawRouteData.Extensions.GetOrCreateExtension<RouteExtension>();

            this.Segments = new List<LoadedGpxFileTourSegmentInfo>(1);
            this.Segments.Add(new LoadedGpxFileTourSegmentInfo(rawRouteData));

            this.CalculateDistances();
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

            this.CalculateDistances();
        }

        public void CalculateDistances()
        {
            var distanceMeters = 0.0;
            var elevationUpMeters = 0.0;
            var elevationDownMeters = 0.0;
            foreach (var actSegment in this.Segments)
            {
                if (actSegment.Points.Count <= 1) { continue; }

                var lastPoint = actSegment.Points[0];
                foreach (var actPoint in actSegment.Points.GetRange(1, actSegment.Points.Count -1))
                {
                    distanceMeters += GeoCalculator.CalculateDistanceMeters(
                        lastPoint, actPoint);

                    if (lastPoint.ElevationSpecified && actPoint.ElevationSpecified)
                    {
                        var elevationLast = (double)lastPoint.Elevation!;
                        var elevationAct = (double)actPoint.Elevation!;
                        if (elevationLast.Equals3DigitPrecision(elevationAct)){ }
                        if (elevationAct > elevationLast) { elevationUpMeters += (elevationAct - elevationLast); }
                        else { elevationDownMeters += (elevationLast - elevationAct); }
                    }

                    lastPoint = actPoint;
                }
            }

            this.DistanceKm = distanceMeters / 1000.0;
            this.ElevationUpMeters = elevationUpMeters;
            this.ElevationDownMeters = elevationDownMeters;
        }
    }
}
