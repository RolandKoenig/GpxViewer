using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFileWaypointInfo : ILoadedGpxFileWaypointInfo
    {
        /// <inheritdoc />
        public ILoadedGpxFile File { get; }

        /// <inheritdoc />
        public GpxWaypoint RawWaypointData { get; }

        public LoadedGpxFileWaypointInfo(LoadedGpxFile gpxFile, GpxWaypoint rawWaypoint)
        {
            this.File = gpxFile;
            this.RawWaypointData = rawWaypoint;
        }


    }
}
