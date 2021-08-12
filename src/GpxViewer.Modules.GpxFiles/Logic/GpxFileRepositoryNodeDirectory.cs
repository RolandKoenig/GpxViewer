using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeDirectory : GpxFileRepositoryNode
    {
        public override string NodeText => Path.GetFileName(this.DirectoryPath);

        public override LoadedGpxFile? AssociatedGpxFile => null;

        public string DirectoryPath { get; }

        public GpxFileRepositoryNodeDirectory(string directory)
        {
            this.DirectoryPath = directory;

            foreach (var actDirectory in Directory.GetDirectories(this.DirectoryPath))
            {
                this.ChildNodes.Add(new GpxFileRepositoryNodeDirectory(actDirectory));
            }

            foreach(var actFile in Directory.GetFiles(this.DirectoryPath))
            {
                var actFileExtension = Path.GetExtension(actFile);
                if (!actFileExtension.Equals(".gpx", StringComparison.OrdinalIgnoreCase)){ continue; }

                this.ChildNodes.Add(new GpxFileRepositoryNodeFile(actFile));
            }
        }
    }
}
