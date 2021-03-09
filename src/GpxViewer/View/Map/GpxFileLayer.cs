using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using GpxViewer.Utils;
using FirLib.Formats.Gpx;

namespace GpxViewer.View.Map
{
    public class GpxFileLayer : MemoryLayer
    {
        private VectorStyle _lineStyle;

        public GpxFile GpxFile
        {
            get;
        }

        public GpxFileLayer(GpxFile gpxFile)
        {
            this.GpxFile = gpxFile;

            var linePoints = new List<Mapsui.Geometries.Point>();
            foreach (var actPoint in gpxFile.Tracks[0].Segments[0].Points)
            {
                linePoints.Add(SphericalMercator.FromLonLat(actPoint.Longitude, actPoint.Latitude));
            }

            var lineString = new LineString(linePoints);
            _lineStyle = new VectorStyle
            {
                Fill = null,
                Outline = null,
                Line = { Color = Mapsui.Styles.Color.Black, Width = 4 }
            };

            this.DataSource = new MemoryProvider(new Feature {Geometry = lineString});
            this.Name = "LineStringLayer";
            this.Style = _lineStyle;
        }

        public System.Windows.Media.Color Color
        {
            get => _lineStyle.Line.Color.ToWpfColor();
            set
            {
                _lineStyle.Line.Color = value.ToMapsuiColor();
                this.DataHasChanged();
            }
        }

        public double LineWidth
        {
            get => _lineStyle.Line.Width;
            set
            {
                _lineStyle.Line.Width = value;
                this.DataHasChanged();
            }
        }
    }
}
