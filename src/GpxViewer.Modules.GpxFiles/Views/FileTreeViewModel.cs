using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Utils.Collections;
using FirLib.Core.ViewServices;
using GpxViewer.Core.Commands;
using GpxViewer.Core.Patterns;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class FileTreeViewModel : GpxViewerViewModelBase
    {
        // Dependencies
        private GpxFileRepository _repoGpxFiles;
        private IGpxViewerCommands _gpxViewerCommands;

        private FileTreeNodeViewModel? _selectedNode;

        public FileTreeNodeViewModel? SelectedNode
        {
            get => _selectedNode;
            set
            {
                if(_selectedNode != value)
                {
                    _selectedNode = value;
                    this.RaisePropertyChanged();

                    if (_selectedNode != null)
                    {
                        this.Messenger.BeginPublish(
                            new MessageGpxFileRepositoryNodeSelectionChanged(new IGpxFileRepositoryNode[]{ _selectedNode.Model }));
                    }
                    else
                    {
                        this.Messenger.BeginPublish(
                            new MessageGpxFileRepositoryNodeSelectionChanged(null));
                    }
                }
            }
        }

        public TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode> TopLevelNodes { get; }

        public DelegateCommand Command_LoadFile { get; }
        public DelegateCommand Command_LoadDirectory { get; }
        public DelegateCommand Command_CloseAll { get; }

        public FileTreeViewModel(GpxFileRepository fileRepo, IGpxViewerCommands gpxViewerCommands)
        {
            _repoGpxFiles = fileRepo;
            _gpxViewerCommands = gpxViewerCommands;

            this.TopLevelNodes = new TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode>(
                fileRepo.TopLevelNodes,
                nodeModel => new FileTreeNodeViewModel(nodeModel));

            this.Command_LoadFile = new DelegateCommand(this.OnCommand_LoadFile_Execute);
            this.Command_LoadDirectory = new DelegateCommand(this.OnCommand_LoadDirectory_Execute);
            this.Command_CloseAll = new DelegateCommand(
                this.OnCommand_CloseAll_Execute,
                () => this.TopLevelNodes.Count > 0);
        }

        private static void TriggerNodeUIUpdate(FileTreeNodeViewModel currentNode, ILoadedGpxFile filteredFile)
        {
            if(currentNode.AssociatedGpxFile == filteredFile)
            {
                currentNode.RaiseNodeTextChanged();
            }

            foreach(var actChildNode in currentNode.ChildNodes)
            {
                TriggerNodeUIUpdate(actChildNode, filteredFile);
            }
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

        private void OnMessageReceived(MessageTourConfigurationChanged message)
        {
            foreach(var actTopLevelNode in this.TopLevelNodes)
            {
                TriggerNodeUIUpdate(actTopLevelNode, message.Tour.File);
            }
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
                await _repoGpxFiles.LoadFile(new FileOrDirectoryPath(actFile));
            }
        }

        private async void OnCommand_LoadDirectory_Execute()
        {
            var srvLoadDirectory = this.GetViewService<IOpenDirectoryViewService>();
            var selectedPath = await srvLoadDirectory.ShowOpenDirectoryDialogAsync("Load GPX Files");
            if (string.IsNullOrEmpty(selectedPath)) { return; }

            await _repoGpxFiles.LoadDirectory(new FileOrDirectoryPath(selectedPath));
        }

        private void OnCommand_CloseAll_Execute()
        { 
            _repoGpxFiles.CloseAllFiles();
        }
    }
}
