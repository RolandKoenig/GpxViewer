using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.ObjectPooling;
using FirLib.Formats.Gpx;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal class GpxFileRepositoryNodeFile : GpxFileRepositoryNode
    {
        private string _filePath;
        private Exception? _fileLoadError;

        public override string NodeText
        {
            get
            {
                using(_ = PooledStringBuilders.Current.UseStringBuilder(out var strBuilder))
                {
                    strBuilder.Append(Path.GetFileName(_filePath));
                    if ((this.AssociatedGpxFile != null) &&
                        (this.AssociatedGpxFile.ContentsChanged))
                    {
                        strBuilder.Append('*');
                    }
                    if (_fileLoadError != null)
                    {
                        strBuilder.Append(" ** Loading Error **");
                    }
                    return strBuilder.ToString();
                }
            }
        }

        public override LoadedGpxFile? AssociatedGpxFile { get; }

        public GpxFileRepositoryNodeFile(string filePath)
        {
            _filePath = filePath;

            try
            {
                this.AssociatedGpxFile = new LoadedGpxFile(GpxFile.Deserialize(filePath));
            }
            catch (Exception e)
            {
                this.AssociatedGpxFile = null;
                _fileLoadError = e;
            }
        }
    }
}
