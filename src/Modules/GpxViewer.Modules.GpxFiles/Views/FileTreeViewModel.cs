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

        public FileTreeNodeViewModel? SelectedNode
        {
            get
            {
                var selectedNode = _repoGpxFiles.SelectedNode;
                if (selectedNode == null) { return null; }

                foreach(var actNodeVM in this.EnumerateAllNodes(this.TopLevelNodes))
                {
                    if (actNodeVM.Model == selectedNode) { return actNodeVM; }
                }

                return null;
            }
            set
            {
                if (this.SelectedNode != value)
                {
                    _repoGpxFiles.SelectedNode = value?.Model;
                }

                this.Command_Save.RaiseCanExecuteChanged();
                this.Command_SaveAll.RaiseCanExecuteChanged();
            }
        }

        public TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode> TopLevelNodes { get; }

        public DelegateCommand Command_LoadFile { get; }
        public DelegateCommand Command_LoadDirectory { get; }
        public DelegateCommand Command_Save { get; }
        public DelegateCommand Command_SaveAll { get; }
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

            this.Command_Save = new DelegateCommand(
                this.OnCommand_Save_Execute,
                () => _repoGpxFiles.SelectedNode?.ContentsChanged ?? false);
            this.Command_SaveAll = new DelegateCommand(
                this.OnCommand_SaveAll_Execute,
                () => _repoGpxFiles.EnumerateNodesDeep().Any(actNode => actNode.AssociatedGpxFile?.ContentsChanged ?? false));

            this.Command_CloseAll = new DelegateCommand(
                this.OnCommand_CloseAll_Execute,
                () => this.TopLevelNodes.Count > 0);
        }

        public IEnumerable<FileTreeNodeViewModel> EnumerateAllNodes(IEnumerable<FileTreeNodeViewModel> currentLevel)
        {
            foreach (var actNode in currentLevel)
            {
                yield return actNode;

                foreach (var actNodeChild in this.EnumerateAllNodes(actNode.ChildNodes))
                {
                    yield return actNodeChild;
                }
            }
        }

        public void NotifyFileTreeNodeDoubleClick(FileTreeNodeViewModel nodeViewModel)
        {
            base.Messenger.Publish(
                new MessageFocusFileRepositoryNodeRequest(nodeViewModel.Model));
        }

        private static bool TriggerNodeUIUpdate(FileTreeNodeViewModel currentNode, ILoadedGpxFile filteredFile)
        {
            var result = false;
            if(currentNode.AssociatedGpxFile == filteredFile)
            {
                currentNode.RaiseNodeTextChanged();
                result = true;
            }

            foreach(var actChildNode in currentNode.ChildNodes)
            {
                var childNodeUpdated = TriggerNodeUIUpdate(actChildNode, filteredFile);
                if(childNodeUpdated){ currentNode.RaiseNodeTextChanged(); }
            }
            return result;
        }

        /// <inheritdoc />
        protected override void OnMvvmViewAttached()
        {
            base.OnMvvmViewAttached();

            _repoGpxFiles.SelectedNodeChanged += this.OnGpxFileRepository_SelectedNodeChanged;

            _gpxViewerCommands.LoadFile.RegisterCommand(this.Command_LoadFile);
            _gpxViewerCommands.LoadDirectory.RegisterCommand(this.Command_LoadDirectory);
            _gpxViewerCommands.Save.RegisterCommand(this.Command_Save);
            _gpxViewerCommands.SaveAll.RegisterCommand(this.Command_SaveAll);
            _gpxViewerCommands.CloseAll.RegisterCommand(this.Command_CloseAll);
        }

        /// <inheritdoc />
        protected override void OnMvvmViewDetaching()
        {
            base.OnMvvmViewDetaching();

            _repoGpxFiles.SelectedNodeChanged -= this.OnGpxFileRepository_SelectedNodeChanged;

            _gpxViewerCommands.LoadFile.UnregisterCommand(this.Command_LoadFile);
            _gpxViewerCommands.LoadDirectory.UnregisterCommand(this.Command_LoadDirectory);
            _gpxViewerCommands.Save.UnregisterCommand(this.Command_Save);
            _gpxViewerCommands.SaveAll.UnregisterCommand(this.Command_SaveAll);
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

            this.Command_Save.RaiseCanExecuteChanged();
            this.Command_SaveAll.RaiseCanExecuteChanged();
        }

        private void OnGpxFileRepository_SelectedNodeChanged(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged(nameof(this.SelectedNode));

            var newlySelectedNode = _repoGpxFiles.SelectedNode;
            if (newlySelectedNode != null)
            {
                this.Messenger.BeginPublish(
                    new MessageGpxFileRepositoryNodeSelectionChanged(new IGpxFileRepositoryNode[]{ newlySelectedNode }));
            }
            else
            {
                this.Messenger.BeginPublish(
                    new MessageGpxFileRepositoryNodeSelectionChanged(null));
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

            GpxFileRepositoryNodeFile? lastFile = null;
            foreach(var actFile in fileList)
            {
                lastFile = await _repoGpxFiles.LoadFile(new FileOrDirectoryPath(actFile));
            }
            if (lastFile != null) { _repoGpxFiles.SelectedNode = lastFile; }
        }

        private async void OnCommand_LoadDirectory_Execute()
        {
            var srvLoadDirectory = this.GetViewService<IOpenDirectoryViewService>();
            var selectedPath = await srvLoadDirectory.ShowOpenDirectoryDialogAsync("Load GPX Files");
            if (string.IsNullOrEmpty(selectedPath)) { return; }

            _repoGpxFiles.SelectedNode = await _repoGpxFiles.LoadDirectory(new FileOrDirectoryPath(selectedPath));
        }

        private async void OnCommand_Save_Execute()
        {
            var selectedNode = _repoGpxFiles.SelectedNode;
            if (selectedNode == null) { return; }

            var savedTours = new HashSet<ILoadedGpxFileTourInfo>();
            try
            {
                await foreach (var actSaved in selectedNode.SaveAsync())
                {
                    foreach (var actSavedTour in actSaved.GetAllAssociatedTours())
                    {
                        if(savedTours.Contains(actSavedTour)){ continue; }
                        savedTours.Add(actSavedTour);
                    }
                }
            }
            finally
            {
                foreach (var actSavedTour in savedTours)
                {
                    base.Messenger.Publish(new MessageTourConfigurationChanged(actSavedTour));
                }
            }
        }

        private async void OnCommand_SaveAll_Execute()
        {
            var savedTours = new HashSet<ILoadedGpxFileTourInfo>();
            try
            {
                foreach (var actNode in _repoGpxFiles.TopLevelNodes)
                {
                    await foreach (var actSaved in actNode.SaveAsync())
                    {
                        foreach (var actSavedTour in actSaved.GetAllAssociatedTours())
                        {
                            if(savedTours.Contains(actSavedTour)){ continue; }
                            savedTours.Add(actSavedTour);
                        }
                    }
                }
            }
            finally
            {
                foreach (var actSavedTour in savedTours)
                {
                    base.Messenger.Publish(new MessageTourConfigurationChanged(actSavedTour));
                }
            }
        }

        private void OnCommand_CloseAll_Execute()
        { 
            _repoGpxFiles.CloseAllFiles();
        }
    }
}
