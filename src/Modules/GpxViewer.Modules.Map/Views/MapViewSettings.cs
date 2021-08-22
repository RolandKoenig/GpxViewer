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

        public RenderMode RenderMode
        {
            get
            {
                if(Enum.TryParse(typeof(RenderMode), _moduleConfig.RenderMethod, out var result))
                {
                    return (RenderMode)result!;
                }
                return RenderMode.Wpf;
            }
            set
            {
                var valueStr = value.ToString();
                if (valueStr != _moduleConfig.RenderMethod)
                {
                    _moduleConfig.RenderMethod = valueStr;
                    this.RaisePropertyChanged(nameof(this.RenderMode));
                }
            }
        }

        public MapViewSettings(MapModuleConfiguration moduleConfig)
        {
            _moduleConfig = moduleConfig;
        }
    }
}
