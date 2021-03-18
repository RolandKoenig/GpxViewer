using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using GpxViewer.Core.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class LoadedGpxFile : ILoadedGpxFile
    {
        private GpxFile _gpxFile;

        public LoadedGpxFile(GpxFile gpxFile)
        {
            _gpxFile = gpxFile;
        }
    }
}
