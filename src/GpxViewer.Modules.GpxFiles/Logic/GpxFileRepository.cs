using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Messaging;
using GpxViewer.Modules.GpxFiles.Interface.Messages;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepository : IGpxFileRepository
    {
        private IFirLibMessagePublisher _msgPublisher;

        public ObservableCollection<GpxFileRepositoryNode> TopLevelNodes { get; } = new();

        public GpxFileRepository(IFirLibMessagePublisher msgPublisher)
        {
            _msgPublisher = msgPublisher;
        }

        public async Task<GpxFileRepositoryNodeFile> LoadFile(string filePath)
        {
            GpxFileRepositoryNodeFile? loadedFile = null;
            await Task.Factory.StartNew(() =>
            {
                loadedFile = new GpxFileRepositoryNodeFile(filePath);
            });
            this.TopLevelNodes.Add(loadedFile!);

            _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this, new IGpxFileRepositoryNode[]{ loadedFile! }, null));
            return loadedFile!;
        }

        public async Task<GpxFileRepositoryNodeDirectory> LoadDirectory(string directoryPath)
        {
            GpxFileRepositoryNodeDirectory? loadedDir = null;
            await Task.Factory.StartNew(() =>
            {
                loadedDir = new GpxFileRepositoryNodeDirectory(directoryPath);
            });
            this.TopLevelNodes.Add(loadedDir!);

            _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this,  new IGpxFileRepositoryNode[]{ loadedDir! }, null));
            return loadedDir!;
        }

        public IEnumerable<LoadedGpxFile> GetAllLoadedGpxFiles()
        {
            yield break;
        }

        /// <inheritdoc />
        public IEnumerable<ILoadedGpxFile> GetAllSelectedGpxFiles()
        {
            return this.GetAllLoadedGpxFiles();
        }

        public void CloseAllFiles()
        {
            var prevItems = this.TopLevelNodes.ToArray();
            this.TopLevelNodes.Clear();

            _msgPublisher.Publish(new MessageGpxFileRepositoryContentsChanged(this, null, prevItems));
        }
    }
}
