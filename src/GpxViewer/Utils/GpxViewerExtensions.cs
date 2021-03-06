using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Utils
{
    public static class GpxViewerExtensions
    {
        public static System.Windows.Media.Color ToWpfColor(this Mapsui.Styles.Color mapuiColor)
        {
            return new System.Windows.Media.Color()
            {
                R = (byte)mapuiColor.R,
                G = (byte)mapuiColor.G,
                B = (byte)mapuiColor.B,
                A = (byte)mapuiColor.A
            };
        }

        public static Mapsui.Styles.Color ToMapsuiColor(this System.Windows.Media.Color wpfColor)
        {
            return new Mapsui.Styles.Color(
                wpfColor.R, wpfColor.G, wpfColor.B, wpfColor.A);
        }
    }
}
