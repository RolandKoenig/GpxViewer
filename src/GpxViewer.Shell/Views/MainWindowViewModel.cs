using FirLib.Core.Patterns.Mvvm;
using GpxViewer.Core.Commands;
using Prism.Mvvm;

namespace GpxViewer.Shell.Views
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title = "Prism Application";

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
