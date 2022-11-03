using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirLib.Core;
using FirLib.Core.Utils.Collections;
using FirLib.Core.ViewServices;
using GpxViewer.Core.Commands;
using GpxViewer.Core.Messages;
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
                this.Command_Close.RaiseCanExecuteChanged();
                this.Command_DeselectAll.RaiseCanExecuteChanged();
            }
        }

        public TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode> TopLevelNodes { get; }

        public DelegateCommand Command_LoadFile { get; }
        public DelegateCommand Command_LoadDirectory { get; }
        public DelegateCommand Command_Save { get; }
        public DelegateCommand Command_SaveAll { get; }
        public DelegateCommand Command_Close { get; }
        public DelegateCommand Command_CloseAll { get; }

        public DelegateCommand Command_DeselectAll { get; }

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
                () => this.OnCommand_Save_ExecuteAsync().FireAndForget(),
                () => _repoGpxFiles.SelectedNode?.ContentsChanged ?? false);
            this.Command_SaveAll = new DelegateCommand(
                () => this.OnCommand_SaveAll_ExecuteAsync().FireAndForget(),
                () => _repoGpxFiles.EnumerateNodesDeep().Any(actNode => actNode.ContentsChanged));

            this.Command_Close = new DelegateCommand(
                this.OnCommand_Close_Execute,
                () => _repoGpxFiles.SelectedNode != null);
            this.Command_CloseAll = new DelegateCommand(
                this.OnCommand_CloseAll_Execute,
                () => this.TopLevelNodes.Count > 0);

            this.Command_DeselectAll = new DelegateCommand(
                () => this.SelectedNode = null,
                () => this.SelectedNode != null);
        }

        public void TryNavigateUp()
        {
            static bool FindNodeBeforeSelectedNode(
                FileTreeNodeViewModel selectedNode, 
                ref FileTreeNodeViewModel? lastNode,
                IEnumerable<FileTreeNodeViewModel> nodes)
            {
                foreach (var actNode in nodes)
                {
                    if (actNode == selectedNode)
                    {
                        return lastNode != null;
                    }

                    lastNode = actNode;

                    if (actNode.IsExpanded)
                    {
                        if (FindNodeBeforeSelectedNode(selectedNode, ref lastNode, actNode.ChildNodes))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            if (this.SelectedNode == null)
            {
                this.SelectedNode = this.TopLevelNodes.FirstOrDefault();
                return;
            }

            FileTreeNodeViewModel? lastNode = null;
            if (FindNodeBeforeSelectedNode(this.SelectedNode, ref lastNode, this.TopLevelNodes))
            {
                this.SelectedNode = lastNode;
            }
        }

        public void TryNavigateDown()
        {
            static bool FindNodeAfterSelectedNode(
                FileTreeNodeViewModel selectedNode, 
                ref bool selectedNodeFound,
                ref FileTreeNodeViewModel? followingNode,
                IEnumerable<FileTreeNodeViewModel> nodes)
            {
                foreach (var actNode in nodes)
                {
                    if (selectedNodeFound)
                    {
                        followingNode = actNode;
                        return true;
                    }

                    if (actNode == selectedNode)
                    {
                        selectedNodeFound = true;
                    }
                
                    if (actNode.IsExpanded)
                    {
                        if (FindNodeAfterSelectedNode(selectedNode, ref selectedNodeFound, ref followingNode, actNode.ChildNodes))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            if (this.SelectedNode == null)
            {
                this.SelectedNode = this.TopLevelNodes.FirstOrDefault();
                return;
            }

            var foundSelectedNode = false;
            FileTreeNodeViewModel? followingNode = null;
            if (FindNodeAfterSelectedNode(this.SelectedNode, ref foundSelectedNode, ref followingNode, this.TopLevelNodes))
            {
                this.SelectedNode = followingNode;
            }
        }

        public void TryExpandSelectedNode()
        {
            if (this.SelectedNode == null) { return; }

            this.SelectedNode.IsExpanded = this.SelectedNode.ChildNodes.Count > 0;
        }

        public void TryCollapseSelectedNode()
        {
            if (this.SelectedNode == null) { return; }

            this.SelectedNode.IsExpanded = false;
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

        private static bool TriggerNodeUIUpdate(FileTreeNodeViewModel currentNode, HashSet<GpxFileRepositoryNode> models)
        {
            var result = false;
            if(models.Contains(currentNode.Model))
            {
                currentNode.RaiseNodeTextChanged();
                result = true;
            }

            foreach(var actChildNode in currentNode.ChildNodes)
            {
                var childNodeUpdated = TriggerNodeUIUpdate(actChildNode, models);
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
            _gpxViewerCommands.Close.RegisterCommand(this.Command_Close);
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
            _gpxViewerCommands.Close.UnregisterCommand(this.Command_Close);
            _gpxViewerCommands.CloseAll.UnregisterCommand(this.Command_CloseAll);
        }

        private void OnMessageReceived(MessageGpxFileRepositoryContentsChanged message)
        {
            this.Command_CloseAll.RaiseCanExecuteChanged();

            // Search parent nodes which should be updated
            var nodesToUpdate = new HashSet<GpxFileRepositoryNode>();
            if (message.AddedNodes != null)
            {
                foreach (var actAddedNode in message.AddedNodes)
                {
                    var actParent = (actAddedNode as GpxFileRepositoryNode)?.Parent;
                    while(actParent != null)
                    {
                        nodesToUpdate.Add(actParent);
                        actParent = actParent.Parent;
                    }
                }
            }
            if (message.RemovedNodes != null)
            {
                foreach (var actRemovedNode in message.RemovedNodes)
                {
                    var actParent = (actRemovedNode as GpxFileRepositoryNode)?.Parent;
                    while(actParent != null)
                    {
                        nodesToUpdate.Add(actParent);
                        actParent = actParent.Parent;
                    }
                }
            }

            // Stop here if there is nothing to update
            if (nodesToUpdate.Count == 0) { return; }

            // Update all viewmodels that may have changed
            foreach (var actTopLevelNode in this.TopLevelNodes)
            {
                TriggerNodeUIUpdate(actTopLevelNode, nodesToUpdate);
            }
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

        private void OnMessageReceived(MessageGpxViewerSaveBeforeExit_Preview message)
        {
            if (_repoGpxFiles.ContentsChanged)
            {
                message.AnyUnsavedChanges = true;
            }
        }

        private void OnMessageReceived(MessageGpxViewerSaveBeforeExit_Save message)
        {
            message.SaveTasks.Add(this.OnCommand_SaveAll_ExecuteAsync());
        }

        private void OnMessageReceived(MessageSelectGpxTourRequest message)
        {
            if (message.TourToSelect == null)
            {
                this.SelectedNode = null;
                return;
            }

            var correspondingNode = this.EnumerateAllNodes(this.TopLevelNodes)
                .FirstOrDefault(x =>
                {
                    var currentFile = x.Model.GetAssociatedGpxFile();
                    if (currentFile == null) { return false; }

                    return currentFile.Tours.Contains(message.TourToSelect);
                });
            
            this.SelectedNode = correspondingNode;
        }

        private void OnGpxFileRepository_SelectedNodeChanged(object? sender, EventArgs e)
        {
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

            this.SelectedNode = this.SelectedNode;
            this.RaisePropertyChanged(nameof(this.SelectedNode));
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

        private async Task OnCommand_Save_ExecuteAsync()
        {
            var selectedNode = _repoGpxFiles.SelectedNode;
            if (selectedNode == null) { return; }

            // Search for savable node up the tree
            while (selectedNode is { CanSave: false })
            {
                selectedNode = selectedNode.Parent;
            }
            if (selectedNode == null) { return; }

            // Save current node
            var savedTours = new HashSet<ILoadedGpxFileTourInfo>();
            try
            {
                await foreach (var actSaved in selectedNode.SaveAsync())
                {
                    foreach (var actSavedTour in actSaved.GetAssociatedToursDeep())
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

        private async Task OnCommand_SaveAll_ExecuteAsync()
        {
            var savedTours = new HashSet<ILoadedGpxFileTourInfo>();
            try
            {
                foreach (var actNode in _repoGpxFiles.TopLevelNodes)
                {
                    await foreach (var actSaved in actNode.SaveAsync())
                    {
                        foreach (var actSavedTour in actSaved.GetAssociatedToursDeep())
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

        private async void OnCommand_Close_Execute()
        {
            var selectedNode = _repoGpxFiles.SelectedNode;
            if (selectedNode == null) { return; }

            if (selectedNode.ContentsChanged)
            {
                var srvMessageBox = this.GetViewService<IMessageBoxService>();
                var msgResult = await srvMessageBox.ShowAsync(
                    "RK Gpx Viewer", "Save changes before close?", MessageBoxButtons.YesNoCancel);
                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        await this.OnCommand_Save_ExecuteAsync();
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

            _repoGpxFiles.Close(selectedNode);
        }

        private async void OnCommand_CloseAll_Execute()
        {
            if (_repoGpxFiles.ContentsChanged)
            {
                var srvMessageBox = this.GetViewService<IMessageBoxService>();
                var msgResult = await srvMessageBox.ShowAsync(
                    "RK Gpx Viewer", "Save changes before close?", MessageBoxButtons.YesNoCancel);
                switch (msgResult)
                {
                    case MessageBoxResult.Yes:
                        await this.OnCommand_SaveAll_ExecuteAsync();
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

            _repoGpxFiles.CloseAll();
        }
    }
}
