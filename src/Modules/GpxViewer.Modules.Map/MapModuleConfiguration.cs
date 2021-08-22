using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Modules.Map
{
    internal class MapModuleConfiguration
    {
        public double LastViewportMinX { get; set; }

        public double LastViewportMinY { get; set; }

        public double LastViewportMaxX { get; set; }

        public double LastViewportMaxY { get; set; }

        public string RenderMethod { get; set; } = string.Empty;
    }
}
