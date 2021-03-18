using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepository : IGpxFileRepository
    {
        public ObservableCollection<GpxFileRepositoryNode> TopLevelNodes { get; } = new();

        public async Task<GpxFileRepositoryNodeFile> LoadFile(string filePath)
        {
            GpxFileRepositoryNodeFile? loadedFile = null;
            await Task.Factory.StartNew(() =>
            {
                loadedFile = new GpxFileRepositoryNodeFile(filePath);
            });

            this.TopLevelNodes.Add(loadedFile!);
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
            return loadedDir!;
        }

        public IEnumerable<LoadedGpxFile> GetAllLoadedGpxFiles()
        {
            yield break;
        }

        /// <inheritdoc />
        public IEnumerable<ILoadedGpxFile> GetAllSelectedGpxFiles()
        {
            return (IEnumerable<ILoadedGpxFile>)this.GetAllLoadedGpxFiles();
        }
    }
}
