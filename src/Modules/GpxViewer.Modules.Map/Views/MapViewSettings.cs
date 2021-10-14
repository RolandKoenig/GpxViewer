using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns;
using Mapsui.UI.Wpf;

namespace GpxViewer.Modules.Map.Views
{
    internal class MapViewSettings : PropertyChangedBase
    {
        private MapModuleConfiguration _moduleConfig;

        public MapViewSettings(MapModuleConfiguration moduleConfig)
        {
            _moduleConfig = moduleConfig;
        }
    }
}
