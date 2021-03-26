using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Utils.Collections;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Logic;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class FileTreeNodeViewModel : GpxViewerViewModelBase
    {
        public GpxFileRepositoryNode Model { get; }

        public string NodeText => this.Model.NodeText;

        public LoadedGpxFile? AssociatedGpxFile => this.Model.AssociatedGpxFile;

        public TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode> ChildNodes { get; }

        public FileTreeNodeViewModel(GpxFileRepositoryNode model)
        {
            this.Model = model;
            this.ChildNodes = new TransformedObservableCollection<FileTreeNodeViewModel, GpxFileRepositoryNode>(
                model.ChildNodes,
                nodeModel => new FileTreeNodeViewModel(nodeModel));
        }

        public void RaiseNodeTextChanged()
        {
            this.RaisePropertyChanged(nameof(this.NodeText));
        }
    }
}
