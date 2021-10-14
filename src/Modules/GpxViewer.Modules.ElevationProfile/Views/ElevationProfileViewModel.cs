using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Formats.Gpx;
using GpxViewer.Core.Patterns;
using GpxViewer.Core.Util;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace GpxViewer.Modules.ElevationProfile.Views
{
    internal class ElevationProfileViewModel : GpxViewerViewModelBase
    {
        private LineSeries _lineSeries;

        public SeriesCollection SeriesCollection { get; }
        public Visibility ElevationProfileVisibility => _lineSeries.Values != null ? Visibility.Visible : Visibility.Collapsed;

        public ElevationProfileViewModel()
        {
            _lineSeries = new LineSeries();
            _lineSeries.Values = null;
            _lineSeries.PointGeometrySize = 0.0;
            _lineSeries.LineSmoothness = 0.0;

            this.SeriesCollection = new SeriesCollection();
            this.SeriesCollection.Add(_lineSeries);
        }

        private void RecreateChartValues(ILoadedGpxFileTourInfo? singleSelectedTour)
        {
            _lineSeries.Values = null;

            if (singleSelectedTour != null)
            {
                // Build a list of ObservablePoints before adding them to ChartValues<ObservablePoint> collection because of better performance
                // see https://lvcharts.net/App/examples/v1/Wpf/Performance%20Tips

                var actDistanceM = 0.0;
                var generatedChartValues = new List<ObservablePoint>();
                foreach (var actSegment in singleSelectedTour.Segments)
                {
                    GpxWaypoint? lastPoint = null;
                    foreach (var actPoint in actSegment.Points)
                    {
                        if (lastPoint == null)
                        {
                            lastPoint = actPoint;
                            generatedChartValues.Add(new ObservablePoint(
                                actDistanceM / 1000.0, 
                                actPoint.Elevation ?? 0.0));
                            continue;
                        }

                        actDistanceM += GeoCalculator.CalculateDistanceMeters(lastPoint, actPoint);
                        generatedChartValues.Add(new ObservablePoint(
                            actDistanceM / 1000.0, 
                            actPoint.Elevation ?? 0.0));

                        lastPoint = actPoint;
                    }
                }

                var chartValues = new ChartValues<ObservablePoint>();
                chartValues.AddRange(generatedChartValues);
                _lineSeries.Values = chartValues;
            }

            this.RaisePropertyChanged(nameof(this.ElevationProfileVisibility));
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            ILoadedGpxFileTourInfo? singleSelectedTour = null;
            if (message.SelectedNodes != null)
            {
                foreach(var actSelectedNode in message.SelectedNodes)
                {
                    foreach (var actTour in actSelectedNode.GetAssociatedToursDeep())
                    {
                        if (singleSelectedTour != null)
                        {
                            this.RecreateChartValues(null);
                            return;
                        }
                        singleSelectedTour = actTour;
                    }
                }
            }
            this.RecreateChartValues(singleSelectedTour);
        }
    }
}
