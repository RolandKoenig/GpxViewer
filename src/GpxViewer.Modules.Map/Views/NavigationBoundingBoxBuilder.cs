using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Mapsui.Geometries;

namespace GpxViewer.Modules.Map.Views
{
    internal class NavigationBoundingBoxBuilder
    {
        private BoundingBox? _currentBox;

        public bool CanBuildBoundingBox => _currentBox != null;

        public void AddGeometry(IGeometry geometry)
        {
            if (_currentBox == null) { _currentBox = geometry.BoundingBox; }
            else { _currentBox = _currentBox.Join(geometry.BoundingBox); }
        }

        public BoundingBox? TryBuild()
        {
            return _currentBox?.Grow(
                _currentBox.Width * 0.1, _currentBox.Height * 0.1);
        }
    }
}
