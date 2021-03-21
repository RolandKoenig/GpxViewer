using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.ViewServices;
using GpxViewer.Core.Commands;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class FileTreeViewModel : GpxViewerViewModelBase
    {
        // Dependencies
        private GpxFileRepository _repoGpxFiles;
        private IGpxViewerCommands _gpxViewerCommands;

        private GpxFileRepositoryNode? _selectedNode;

        public GpxFileRepositoryNode? SelectedNode
        {
            get => _selectedNode;
            set
            {
                if(_selectedNode != value)
                {
                    _selectedNode = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public ObservableCollection<GpxFileRepositoryNode> TopLevelNodes => _repoGpxFiles.TopLevelNodes;

        public DelegateCommand Command_LoadFile { get; }
        public DelegateCommand Command_LoadDirectory { get; }
        public DelegateCommand Command_CloseAll { get; }

        public FileTreeViewModel(GpxFileRepository fileRepo, IGpxViewerCommands gpxViewerCommands)
        {
            _repoGpxFiles = fileRepo;
            _gpxViewerCommands = gpxViewerCommands;

            this.Command_LoadFile = new DelegateCommand(this.OnCommand_LoadFile_Execute);
            this.Command_LoadDirectory = new DelegateCommand(this.OnCommand_LoadDirectory_Execute);
            this.Command_CloseAll = new DelegateCommand(
                this.OnCommand_CloseAll_Execute,
                () => this.TopLevelNodes.Count > 0);
        }

        /// <inheritdoc />
        protected override void OnMvvmViewAttached()
        {
            base.OnMvvmViewAttached();

            _gpxViewerCommands.LoadFile.RegisterCommand(this.Command_LoadFile);
            _gpxViewerCommands.LoadDirectory.RegisterCommand(this.Command_LoadDirectory);
            _gpxViewerCommands.CloseAll.RegisterCommand(this.Command_CloseAll);
        }

        /// <inheritdoc />
        protected override void OnMvvmViewDetaching()
        {
            base.OnMvvmViewDetaching();

            _gpxViewerCommands.LoadFile.UnregisterCommand(this.Command_LoadFile);
            _gpxViewerCommands.LoadDirectory.UnregisterCommand(this.Command_LoadDirectory);
            _gpxViewerCommands.CloseAll.UnregisterCommand(this.Command_CloseAll);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryContentsChanged message)
        {
            this.Command_CloseAll.RaiseCanExecuteChanged();
        }

        private async void OnCommand_LoadFile_Execute()
        {
            var srvLoadFileDialog = this.GetViewService<IOpenFileViewService>();
            var fileList =  await srvLoadFileDialog.ShowOpenMultipleFilesDialogAsync(
                new FileDialogFilter[]
                {
                    new("GPX File", "gpx")
                },
                "Load GPX Files");
            if (fileList == null) { return; }

            foreach(var actFile in fileList)
            {
                await _repoGpxFiles.LoadFile(actFile);
            }
        }

        private void OnCommand_LoadDirectory_Execute()
        {
            throw new NotImplementedException();
        }

        private void OnCommand_CloseAll_Execute()
        { 
            _repoGpxFiles.CloseAllFiles();
        }
    }
}
