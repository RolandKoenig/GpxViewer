using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;

namespace GpxViewer.Modules.ElevationProfile.Views
{
    internal partial class ChartToolTipView : UserControl, IChartTooltip
    {
        private TooltipData? _data;

        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <inheritdoc />
        public TooltipData? Data
        {
            get => _data;
            set
            {
                _data = value;
                if ((_data != null) &&
                    (_data.Points.Count > 0))
                {
                    var point = _data.Points[0];
                    this.CtrlDistance.Text = point.ChartPoint.X.ToString("N1");
                    this.CtrlElevation.Text = point.ChartPoint.Y.ToString("N0");
                }
            }
        }

        /// <inheritdoc />
        public TooltipSelectionMode? SelectionMode { get; set; }

        public ChartToolTipView()
        {
            this.InitializeComponent();
        }

        private void RaisePropertyChanged(string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
