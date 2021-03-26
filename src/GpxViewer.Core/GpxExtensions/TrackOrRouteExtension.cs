using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Core.GpxExtensions
{
    public abstract class TrackOrRouteExtension
    {
        public GpxTrackState State { get; set; } = GpxTrackState.Unknown;
    }
}
