using FirLib.Core.Patterns;
using GpxViewer.Core.Commands;
using GpxViewer.Core.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Shell.Interface.Services;
using Svg.Model.Primitives;

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

        public MainWindowViewModel(
            ShellModuleConfiguration config, 
            IGpxViewerCommands gpxViewerCommands, IGpxViewerSkinService skinService)
        {
            this.Configuration = config;
            this.GpxViewerCommands = gpxViewerCommands;

            // Apply initial skin
            if ((!string.IsNullOrEmpty(config.Skin)) &&
                (Enum.TryParse(typeof(AppSkin), config.Skin, true, out var parseResult)) &&
                (parseResult is AppSkin configuredSkin))
            {
                skinService.Skin = configuredSkin;
            }

            // Handle skin change
            this.Command_SetSkin = new DelegateCommand<string>(arg =>
            {
                skinService.Skin = Enum.Parse<AppSkin>(arg);
                config.Skin = skinService.Skin.ToString();
            });
        }

        public void NotifyOSFileDrop(IEnumerable<string> fileDropItems)
        {
            base.Messenger.Publish(
                new MessageLoadGpxFilesRequest(
                    fileDropItems.Where(System.IO.File.Exists), 
                    fileDropItems.Where(System.IO.Directory.Exists)));
        }
    }
}
