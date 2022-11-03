using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFile : ILoadedGpxFile
    {
        public GpxFile RawGpxFile { get; }

        public string FileName { get; }

        public List<LoadedGpxFileTourInfo> Tours { get; }

        public List<LoadedGpxFileWaypointInfo> Waypoints { get; }

        IEnumerable<ILoadedGpxFileTourInfo> ILoadedGpxFile.Tours => this.Tours;

        IEnumerable<ILoadedGpxFileWaypointInfo> ILoadedGpxFile.Waypoints => this.Waypoints;

        public bool ContentsChanged { get; set; }

        public LoadedGpxFile(string fileName, GpxFile gpxFile)
        {
            this.FileName = fileName;
            this.RawGpxFile = gpxFile;

            this.Waypoints = new List<LoadedGpxFileWaypointInfo>(gpxFile.Waypoints.Count);
            foreach (var actRawWaypointInfo in gpxFile.Waypoints)
            {
                this.Waypoints.Add(new LoadedGpxFileWaypointInfo(this, actRawWaypointInfo));
            }

            this.Tours = new List<LoadedGpxFileTourInfo>();
            foreach (var actRawRouteInfo in gpxFile.Routes)
            {
                this.Tours.Add(new LoadedGpxFileTourInfo(this, actRawRouteInfo));
            }
            foreach(var actRawTrackData in gpxFile.Tracks)
            {
                this.Tours.Add(new LoadedGpxFileTourInfo(this, actRawTrackData));
            }
        }
    }
}
