using GpxViewer.Core.Commands;
using GpxViewer.Core.Patterns;

namespace GpxViewer.Shell.Views
{
    public class MainWindowViewModel : GpxViewerViewModelBase
    {
        private string _title = "RK GPX Viewer";

        public string Title
        {
            get { return _title; }
            set { this.SetProperty(ref _title, value); }
        }

        public IGpxViewerCommands GpxViewerCommands { get; }

        public MainWindowViewModel(IGpxViewerCommands gpxViewerCommands)
        {
            this.GpxViewerCommands = gpxViewerCommands;
        }
    }
}
