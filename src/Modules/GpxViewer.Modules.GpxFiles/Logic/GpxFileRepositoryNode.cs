using System.Collections.Generic;
using System.Collections.ObjectModel;
using FirLib.Core.Patterns.ObjectPooling;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal abstract class GpxFileRepositoryNode : IGpxFileRepositoryNode
    {
        public ObservableCollection<GpxFileRepositoryNode> ChildNodes { get; } = new();

        public string NodeText
        {
            get
            {
                using (_ = PooledStringBuilders.Current.UseStringBuilder(out var strBuilder))
                {
                    strBuilder.Append(this.GetNodeText());
                    if (this.ContentsChanged) { strBuilder.Append('*'); }
                    return strBuilder.ToString();
                }
            }
        }

        public abstract LoadedGpxFile? AssociatedGpxFile { get; }

        public bool ContentsChanged
        {
            get
            {
                if (this.AreThisNodesContentsChanged()) { return true; }
                foreach (var actChildNode in this.ChildNodes)
                {
                    if (actChildNode.ContentsChanged) { return true; }
                }
                return false;
            }
        }

        /// <summary>
        /// This method checks only this node, not child nodes.
        /// </summary>
        protected abstract bool AreThisNodesContentsChanged();

        protected abstract string GetNodeText();

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

        /// <inheritdoc />
        public IEnumerable<ILoadedGpxFileTourInfo> GetAllAssociatedTours()
        {
            if (this.AssociatedGpxFile != null)
            {
                foreach(var actTour in this.AssociatedGpxFile.Tours)
                {
                    yield return actTour;
                }
            }

            foreach (var actChildNode in this.ChildNodes)
            {
                foreach(var actTour in actChildNode.GetAllAssociatedTours())
                {
                    yield return actTour;
                }
            }
        }
    }
}
