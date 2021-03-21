﻿using GpxViewer.Core.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal abstract class GpxFileRepositoryNode : IGpxFileRepositoryNode
    {
        public ObservableCollection<GpxFileRepositoryNode> ChildNodes { get; } = new();

        public abstract string NodeText { get; }

        public abstract LoadedGpxFile? AssociatedGpxFile { get; }

        /// <inheritdoc />
        public IEnumerable<ILoadedGpxFile> GetAllAssociatedGpxFiles()
        {
            if (this.AssociatedGpxFile != null) { yield return this.AssociatedGpxFile; }

            foreach (var actChildNode in this.ChildNodes)
            {
                foreach(var actAssociatedGpxFile in actChildNode.GetAllAssociatedGpxFiles())
                {
                    yield return actAssociatedGpxFile;
                }
            }
        }
    }
}