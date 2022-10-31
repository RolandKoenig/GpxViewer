using System.Windows;
using System.Windows.Controls;

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
