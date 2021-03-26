using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;
using Mapsui.Geometries;

namespace GpxViewer.Modules.GpxFiles.Interface.Model
{
    public interface ILoadedGpxFile
    {
        GpxFile RawGpxFile { get; }

        IEnumerable<ILoadedGpxFileTrackOrRouteInfo> TracksAndRoutes { get; }

        bool ContentsChanged { get; set; }
    }
}
