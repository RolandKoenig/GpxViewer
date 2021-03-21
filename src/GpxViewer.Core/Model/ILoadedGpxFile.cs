using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using Mapsui.Geometries;

namespace GpxViewer.Core.Model
{
    public interface ILoadedGpxFile
    {
        GpxFile RawGpxFile { get; }

        IEnumerable<ILoadedGpxFileTrackInfo> Tracks { get; }
    }
}
