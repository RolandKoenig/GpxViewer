using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns;
using FirLib.Core.Patterns.ObjectPooling;
using GpxViewer.Core;
using GpxViewer.Core.Patterns;
using GpxViewer.Shell.Utils;

namespace GpxViewer.Shell.Views
{
    internal class RecentlyOpenedViewModel
    {
        public RecentlyOpenedInfo Model { get; }

        public string ShortPath
        {
            get
            {
                if (this.Model.Path.Length < 60) { return this.Model.Path; }

                using (_ = PooledStringBuilders.Current.UseStringBuilder(out var strBuilder, 60))
                {
                    strBuilder.Append(this.Model.Path.Substring(0, 19));
                    strBuilder.Append("...");
                    strBuilder.Append(this.Model.Path.Substring(this.Model.Path.Length - 38));
                    return strBuilder.ToString();
                }
            }
        }

        public string Path => this.Model.Path;

        public GpxViewerIconKind IconKind => this.Model.Type == RecentlyOpenedType.File
            ? GpxViewerIconKind.GpxFile
            : GpxViewerIconKind.Folder;

        public DelegateCommand<RecentlyOpenedInfo> Command_Load { get; }

        public RecentlyOpenedViewModel(RecentlyOpenedInfo recentlyOpened, DelegateCommand<RecentlyOpenedInfo> commandLoad)
        {
            this.Model = recentlyOpened;
            this.Command_Load = commandLoad;
        }
    }
}
