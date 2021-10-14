using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using FirLib.Core.Patterns.ObjectPooling;
using GpxViewer.Modules.GpxFiles.Interface.Model;

namespace GpxViewer.Modules.GpxFiles.Logic
{
    internal abstract class GpxFileRepositoryNode : IGpxFileRepositoryNode
    {
        public ObservableCollection<GpxFileRepositoryNode> ChildNodes { get; } = new();

        public GpxFileRepositoryNode? Parent { get; set; }

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

        public bool ContentsChanged
        {
            get
            {
                if (this.HasThisNodesContentsChanged()) { return true; }
                foreach (var actChildNode in this.ChildNodes)
                {
                    if (actChildNode.ContentsChanged) { return true; }
                }
                return false;
            }
        }

        public abstract bool CanSave { get; }

        public virtual bool HasError => false;

        /// <summary>
        /// This method checks only this node, not child nodes.
        /// </summary>
        protected abstract bool HasThisNodesContentsChanged();

        protected virtual ValueTask SaveThisNodesContentsAsync()
        {
            return ValueTask.CompletedTask;
        }

        protected abstract string GetNodeText();

        public virtual Exception? GetErrorDetails() => null;

        public async IAsyncEnumerable<GpxFileRepositoryNode> SaveAsync()
        {
            if (this.HasThisNodesContentsChanged() && this.CanSave)
            {
                await this.SaveThisNodesContentsAsync();
                if (!this.HasThisNodesContentsChanged())
                {
                    yield return this;
                }
            }

            foreach (var actChild in this.ChildNodes)
            {
                await foreach (var actSaved in actChild.SaveAsync())
                {
                    yield return actSaved;
                }
            }
        }

        public abstract ILoadedGpxFile? GetAssociatedGpxFile();

        public abstract ILoadedGpxFileTourInfo? GetAssociatedTour();

        /// <inheritdoc />
        public IEnumerable<ILoadedGpxFile> GetAssociatedGpxFilesDeep()
        {
            var thisGpxFile = this.GetAssociatedGpxFile();
            if (thisGpxFile != null) { yield return thisGpxFile; }

            foreach (var actChildNode in this.ChildNodes)
            {
                if(actChildNode is GpxFileRepositoryNodeTour){ continue; }

                foreach(var actAssociatedGpxFile in actChildNode.GetAssociatedGpxFilesDeep())
                {
                    yield return actAssociatedGpxFile;
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<ILoadedGpxFileTourInfo> GetAssociatedToursDeep()
        {
            var actLocalTour = this.GetAssociatedTour();
            if (actLocalTour != null)
            {
                yield return actLocalTour;
            }

            foreach (var actChildNode in this.ChildNodes)
            {
                foreach(var actTour in actChildNode.GetAssociatedToursDeep())
                {
                    yield return actTour;
                }
            }
        }
    }
}
