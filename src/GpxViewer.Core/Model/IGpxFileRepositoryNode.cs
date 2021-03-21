using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxViewer.Core.Model
{
    public interface IGpxFileRepositoryNode
    {
        string NodeText { get; }

        IEnumerable<ILoadedGpxFile> GetAllAssociatedGpxFiles();
    }
}
