using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Geometries;

namespace GpxViewer.Modules.Map.Views
{
    internal class RequestNavigateToBoundingBoxEventArgs : EventArgs
    {
        public BoundingBox NavTarget { get; }

        public RequestNavigateToBoundingBoxEventArgs(BoundingBox navTarget)
        {
            this.NavTarget = navTarget;
        }
    }

    internal class RequestCurrentViewportEventArgs : EventArgs
    {
        public BoundingBox? CurrentViewPort { get; set; }
    }
}
