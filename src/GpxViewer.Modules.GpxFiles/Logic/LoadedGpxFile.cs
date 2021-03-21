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

        public List<LoadedGpxFileTrackInfo> Tracks { get; }

        IEnumerable<ILoadedGpxFileTrackInfo> ILoadedGpxFile.Tracks => this.Tracks;

        public LoadedGpxFile(GpxFile gpxFile)
        {
            this.RawGpxFile = gpxFile;
            this.Tracks = new List<LoadedGpxFileTrackInfo>();

            foreach(var actRawTrackData in gpxFile.Tracks)
            {
                this.Tracks.Add(new LoadedGpxFileTrackInfo(actRawTrackData));
            }
        }
    }
}
