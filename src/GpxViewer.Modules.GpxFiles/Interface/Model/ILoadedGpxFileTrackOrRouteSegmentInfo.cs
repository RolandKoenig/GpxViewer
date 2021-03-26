using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Formats.Gpx;

namespace GpxViewer.Modules.GpxFiles.Interface.Model
{
    public interface ILoadedGpxFileTrackOrRouteSegmentInfo
    {
        IEnumerable<GpxWaypoint> Points { get; }
    }
}
