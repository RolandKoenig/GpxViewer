using FirLib.Core.Patterns;
using GpxViewer.Core.Commands;
using GpxViewer.Core.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using FirLib.Core.Infrastructure;
using GpxViewer.Core.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Shell.Interface.Services;
using GpxViewer.Shell.Utils;
using FirLib.Core.ViewServices;

namespace GpxViewer.Shell.Views
{
    internal class MainWindowViewModel : GpxViewerViewModelBase
    {
        private IGpxViewerSkinService _srvSkin;

        public string Title { get; }

        public bool ExitApproved { get; private set; }

        public ShellModuleConfiguration Configuration { get; }

        public IEnumerable<RecentlyOpenedViewModel> RecentlyOpened => 
            this.Configuration.RecentlyOpened.Select(entry => new RecentlyOpenedViewModel(entry, this.Command_LoadRecentlyOpened));

        public bool RecentlyOpenedAvailable => this.Configuration.RecentlyOpened.Count > 0;

        public IGpxViewerCommands GpxViewerCommands { get; }

        public DelegateCommand<string> Command_SetSkin { get; }

        public DelegateCommand<RecentlyOpenedInfo> Command_LoadRecentlyOpened { get; }

        public DelegateCommand Command_Exit { get; }

        public MainWindowViewModel(
            ShellModuleConfiguration config, 
            IGpxViewerCommands gpxViewerCommands, IGpxViewerSkinService skinService)
        {
            this.Configuration = config;
            this.GpxViewerCommands = gpxViewerCommands;

            _srvSkin = skinService;

            // Apply initial skin
            if ((!string.IsNullOrEmpty(config.Skin)) &&
                (Enum.TryParse(typeof(AppSkin), config.Skin, true, out var parseResult)) &&
                (parseResult is AppSkin configuredSkin))
            {
                _srvSkin.Skin = configuredSkin;
            }

            // Update application title string
            this.Title = FirLibApplication.Current.ProductFullName;

            // Handle skin change
            this.Command_SetSkin = new DelegateCommand<string>(this.OnCommand_SetSkin_Execute);
            this.Command_LoadRecentlyOpened = new DelegateCommand<RecentlyOpenedInfo>(this.OnCommand_LoadRecentlyOpened_Execute);
            this.Command_Exit = new DelegateCommand(this.OnCommand_Exit_Execute);
        }

        public void NotifyOSFileDrop(IEnumerable<string> fileDropItems)
        {
            base.Messenger.Publish(
                new MessageLoadGpxFilesRequest(
                    fileDropItems.Where(System.IO.File.Exists), 
                    fileDropItems.Where(System.IO.Directory.Exists)));
        }

        private static void HandleNewRecentlyOpenedEntry(RecentlyOpenedInfo entry, List<RecentlyOpenedInfo> list)
        {
            var entryIndex = -1;
            for (var actIndex = 0; actIndex < list.Count; actIndex++)
            {
                if (list[actIndex].Path == entry.Path)
                {
                    entryIndex = actIndex;
                    break;
                }
            }

            if (entryIndex > 0)
            {
                list.RemoveAt(entryIndex);
                list.Insert(0, entry);
            }
            else if (entryIndex == 0)
            {
                // Nothing to do
            }
            else
            {
                list.Insert(0, entry);
                while(list.Count > 10){ list.RemoveAt(list.Count - 1); }
            }
        }

        private void OnCommand_SetSkin_Execute(string skin)
        {
            _srvSkin.Skin = Enum.Parse<AppSkin>(skin);
            this.Configuration.Skin = _srvSkin.Skin.ToString();
        }

        private void OnCommand_LoadRecentlyOpened_Execute(RecentlyOpenedInfo recentlyOpened)
        {
            switch (recentlyOpened.Type)
            {
                case RecentlyOpenedType.File:
                    this.Messenger.Publish(new MessageLoadGpxFilesRequest(new []{ recentlyOpened.Path }, null));
                    break;

                case RecentlyOpenedType.Directory:
                    this.Messenger.Publish(new MessageLoadGpxFilesRequest(null, new []{ recentlyOpened.Path }));
                    break;
            }
        }

        private async void OnCommand_Exit_Execute()
        {
            var msgSaveBeforeExitPreview = new MessageGpxViewerSaveBeforeExit_Preview();
            this.Messenger.Publish(msgSaveBeforeExitPreview);
            if (msgSaveBeforeExitPreview.AnyUnsavedChanges)
            {
                var srvMessageBox = this.GetViewService<IMessageBoxService>();
                var msgResult = await srvMessageBox.ShowAsync(
                    "RK Gpx Viewer", "Save changes before exit?", MessageBoxButtons.YesNoCancel);
                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        var msgSaveBeforeExit_Save = new MessageGpxViewerSaveBeforeExit_Save();
                        this.Messenger.Publish(msgSaveBeforeExit_Save);
                        foreach (var actSaveTask in msgSaveBeforeExit_Save.SaveTasks)
                        {
                            await actSaveTask;
                        }
                        break;

                    case MessageBoxResult.No:
                        // Just close, no saving
                        break;

                    case MessageBoxResult.Cancel:
                        return;

                    default:
                        throw new ArgumentException($"Unexpected MessageBoxResult {msgResult}");
                }
            }

            this.ExitApproved = true;
            this.CloseWindow(null);
        }

        private void OnMessageReceived(MessageGpxFilesLoaded message)
        {
            foreach (var actLoadedFile in message.FilePaths)
            {
                HandleNewRecentlyOpenedEntry(
                    new RecentlyOpenedInfo()
                    {
                        Path = actLoadedFile.Path,
                        Type = RecentlyOpenedType.File
                    },
                    this.Configuration.RecentlyOpened);
            }
            this.RaisePropertyChanged(nameof(this.RecentlyOpenedAvailable));
            this.RaisePropertyChanged(nameof(this.RecentlyOpened));
        }

        private void OnMessageReceived(MessageGpxDirectoriesLoaded message)
        {
            foreach (var actLoadedDirectory in message.DirectoryPaths)
            {
                HandleNewRecentlyOpenedEntry(
                    new RecentlyOpenedInfo()
                    {
                        Path = actLoadedDirectory.Path,
                        Type = RecentlyOpenedType.Directory
                    }, 
                    this.Configuration.RecentlyOpened);
            }
            this.RaisePropertyChanged(nameof(this.RecentlyOpenedAvailable));
            this.RaisePropertyChanged(nameof(this.RecentlyOpened));
        }
    }
}
