using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Geometries;

namespace GpxViewer.Modules.Map.Views
{
    public class RequestNavigateToBoundingBoxEventArgs : EventArgs
    {
        public BoundingBox NavTarget { get; }

        public RequestNavigateToBoundingBoxEventArgs(BoundingBox navTarget)
        {
            this.NavTarget = navTarget;
        }
    }
}
