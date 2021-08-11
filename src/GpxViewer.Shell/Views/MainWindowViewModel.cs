using FirLib.Core.Patterns;
using GpxViewer.Core.Commands;
using GpxViewer.Core.Patterns;
using System;

namespace GpxViewer.Shell.Views
{
    internal class MainWindowViewModel : GpxViewerViewModelBase
    {
        private string _title = "RK GPX Viewer";

        public string Title
        {
            get { return _title; }
            set { this.SetProperty(ref _title, value); }
        }

        public ShellModuleConfiguration Configuration { get; }

        public IGpxViewerCommands GpxViewerCommands { get; }

        public DelegateCommand<string> Command_SetSkin { get; }

        public MainWindowViewModel(ShellModuleConfiguration config, IGpxViewerCommands gpxViewerCommands)
        {
            this.Configuration = config;
            this.GpxViewerCommands = gpxViewerCommands;

            this.Command_SetSkin = new DelegateCommand<string>(arg => App.CurrentApp.Skin = Enum.Parse<AppSkin>(arg));
        }
    }
}
