using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Core.ValueObjects;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepository : IGpxFileRepository
    {
        private IFirLibMessagePublisher _msgPublisher;
        private GpxFileRepositoryNode? _selectedNode;

        public ObservableCollection<GpxFileRepositoryNode> TopLevelNodes { get; } = new();

        public GpxFileRepositoryNode? SelectedNode
        {
            get => _selectedNode;
            set
            {
                if (_selectedNode != value)
                {
                    _selectedNode = value;
                    this.SelectedNodeChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool ContentsChanged => this.TopLevelNodes.Any(actNode => actNode.ContentsChanged);

        public event EventHandler? SelectedNodeChanged;

        public GpxFileRepository(IFirLibMessagePublisher msgPublisher)
        {
            _msgPublisher = msgPublisher;
        }

        public void AddTopLevelNode(GpxFileRepositoryNode node)
        {
            this.TopLevelNodes.Add(node);
            _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this, new IGpxFileRepositoryNode[]{ node }, null));
        }

        public async Task<GpxFileRepositoryNodeFile> LoadFile(FileOrDirectoryPath filePath)
        {
            GpxFileRepositoryNodeFile result;

            var existingFileNode = this.TryGetFileNode(filePath);
            if (existingFileNode != null)
            { 
                result = existingFileNode;
            }
            else
            {
                GpxFileRepositoryNodeFile? loadedFile = null;
                await Task.Factory.StartNew(() =>
                {
                    loadedFile = new GpxFileRepositoryNodeFile(filePath);
                });
                this.AddTopLevelNode(loadedFile!);

                result = loadedFile!;
            }

            _msgPublisher.Publish(new MessageGpxFilesLoaded(
                new []{ filePath },
                new []{ result }));

            return result;
        }

        public async Task<GpxFileRepositoryNodeDirectory> LoadDirectory(FileOrDirectoryPath directoryPath)
        {
            GpxFileRepositoryNodeDirectory result;

            var existingDirNode = this.TryGetDirectoryNode(directoryPath);
            if (existingDirNode != null)
            {
                result = existingDirNode;
            }
            else
            {
                GpxFileRepositoryNodeDirectory? loadedDir = null;
                await Task.Factory.StartNew(() =>
                {
                    loadedDir = new GpxFileRepositoryNodeDirectory(directoryPath);
                });
                this.AddTopLevelNode(loadedDir!);

                result = loadedDir!;
            }

            _msgPublisher.Publish(new MessageGpxDirectoriesLoaded(
                new []{ directoryPath },
                new []{ result }));

            return result;
        }

        public void CloseAll()
        {
            var prevItems = this.TopLevelNodes.ToArray();
            this.TopLevelNodes.Clear();

            this.SelectedNode = null;

            _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this, null, prevItems));
        }

        public void Close(GpxFileRepositoryNode node)
        {
            if (node.Parent != null)
            {
                // Node is inside the tree as a child
                if (this.SelectedNode == node) { this.SelectedNode = null; }
                node.Parent.ChildNodes.Remove(node);

                _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this, null, new IGpxFileRepositoryNode[]{ node }));
            }
            else
            {
                // Node is a top level node
                if (this.SelectedNode == node) { this.SelectedNode = null; }
                this.TopLevelNodes.Remove(node);
                
                _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this, null, new IGpxFileRepositoryNode[]{ node }));
            }
        }

        public GpxFileRepositoryNodeFile? TryGetFileNode(FileOrDirectoryPath filePath)
        {
            foreach (var actNode in this.EnumerateNodesDeep())
            {
                if(actNode is not GpxFileRepositoryNodeFile actFileNode){ continue; }

                if (actFileNode.FilePath == filePath) { return actFileNode; }
            }
            return null;
        }

        public GpxFileRepositoryNodeDirectory? TryGetDirectoryNode(FileOrDirectoryPath dirPath)
        {
            foreach (var actNode in this.EnumerateNodesDeep())
            {
                if(actNode is not GpxFileRepositoryNodeDirectory actDirNode){ continue; }

                if (actDirNode.DirectoryPath.Path == dirPath.Path) { return actDirNode; }
            }
            return null;
        }

        public IEnumerable<GpxFileRepositoryNode> EnumerateNodesDeep()
        {
            return this.EnumerateNodesDeep(this.TopLevelNodes);
        }

        public IEnumerable<GpxFileRepositoryNode> EnumerateNodesDeep(IEnumerable<GpxFileRepositoryNode> nodeCollection)
        {
            foreach (var actNode in nodeCollection)
            {
                yield return actNode;

                foreach (var actNodeInner in this.EnumerateNodesDeep(actNode.ChildNodes))
                {
                    yield return actNodeInner;
                }
            }
        }
    }
}
