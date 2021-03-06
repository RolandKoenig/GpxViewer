using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.View.Map;

namespace GpxViewer.ViewServices.Rename
{
    interface IRenameGpxFilesViewService : IViewService
    {
        void Rename(IEnumerable<GpxFileViewModel> gpxFiles);
    }
}
