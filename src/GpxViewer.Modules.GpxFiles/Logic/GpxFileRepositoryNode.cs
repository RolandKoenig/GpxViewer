using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpxViewer.Core.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal abstract class GpxFileRepositoryNode : IGpxFileRepositoryNode
    {
        public ObservableCollection<GpxFileRepositoryNode> ChildNodes { get; } = new();

        public abstract string NodeText { get; }

        public abstract LoadedGpxFile? AssociatedGpxFile { get; }
    }
}
