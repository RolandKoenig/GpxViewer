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
        private string _directory;

        public override string NodeText => Path.GetFileName(_directory);

        public GpxFileRepositoryNodeDirectory(string directory)
        {
            _directory = directory;

            foreach (var actDirectory in Directory.GetDirectories(_directory))
            {
                this.ChildNodes.Add(new GpxFileRepositoryNodeDirectory(actDirectory));
            }

            foreach(var actFile in Directory.GetFiles(_directory))
            {
                var actFileExtension = Path.GetExtension(actFile);
                if (!actFileExtension.Equals(".gpx", StringComparison.OrdinalIgnoreCase)){ continue; }

                this.ChildNodes.Add(new GpxFileRepositoryNodeFile(actFile));
            }
        }
    }
}
