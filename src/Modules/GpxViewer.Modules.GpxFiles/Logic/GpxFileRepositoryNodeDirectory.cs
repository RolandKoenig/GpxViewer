using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.ValueObjects;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeDirectory : GpxFileRepositoryNode
    {
        public override string NodeText => Path.GetFileName(this.DirectoryPath.Path);

        public override LoadedGpxFile? AssociatedGpxFile => null;

        public FileOrDirectoryPath DirectoryPath { get; }

        public GpxFileRepositoryNodeDirectory(FileOrDirectoryPath directory)
        {
            this.DirectoryPath = directory;

            foreach (var actDirectory in Directory.GetDirectories(this.DirectoryPath.Path))
            {
                this.ChildNodes.Add(new GpxFileRepositoryNodeDirectory(new FileOrDirectoryPath(actDirectory)));
            }

            foreach(var actFilePath in Directory.GetFiles(this.DirectoryPath.Path))
            {
                var actFileExtension = Path.GetExtension(actFilePath);
                if (!actFileExtension.Equals(".gpx", StringComparison.OrdinalIgnoreCase)){ continue; }

                this.ChildNodes.Add(
                    new GpxFileRepositoryNodeFile(new FileOrDirectoryPath(actFilePath)));
            }
        }
    }
}
