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

        public List<LoadedGpxFileTrackInfo> TracksAndRoutes { get; }

        IEnumerable<ILoadedGpxFileTrackOrRouteInfo> ILoadedGpxFile.TracksAndRoutes => this.TracksAndRoutes;

        public LoadedGpxFile(GpxFile gpxFile)
        {
            this.RawGpxFile = gpxFile;
            this.TracksAndRoutes = new List<LoadedGpxFileTrackInfo>();

            foreach (var actRawRouteInfo in gpxFile.Routes)
            {
                this.TracksAndRoutes.Add(new LoadedGpxFileTrackInfo(actRawRouteInfo));
            }
            foreach(var actRawTrackData in gpxFile.Tracks)
            {
                this.TracksAndRoutes.Add(new LoadedGpxFileTrackInfo(actRawTrackData));
            }
        }
    }
}
