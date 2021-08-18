using System;
using System.Collections.Generic;
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

namespace GpxViewer.Core.Controls
{
    public class GpxViewerIcon : Control
    {
        public static readonly DependencyProperty IconKindProperty = DependencyProperty.Register(
            "IconKind", typeof(GpxViewerIconKind), typeof(GpxViewerIcon), new PropertyMetadata(default(GpxViewerIconKind)));

        public GpxViewerIconKind IconKind
        {
            get { return (GpxViewerIconKind) this.GetValue(IconKindProperty); }
            set { this.SetValue(IconKindProperty, value); }
        }

        static GpxViewerIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GpxViewerIcon), new FrameworkPropertyMetadata(typeof(GpxViewerIcon)));
        }
    }
}
