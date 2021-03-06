using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.View.Map;

namespace GpxViewer.ViewServices.Rename
{
    class WpfRenameGpxFilesViewService : ViewServiceBase, IRenameGpxFilesViewService
    {
        private Window _owner;

        public WpfRenameGpxFilesViewService(Window owner)
        {
            _owner = owner;
        }

        /// <inheritdoc />
        public void Rename(IEnumerable<GpxFileViewModel> gpxFiles)
        {
            var dlgRename = new RenameGpxFilesDialog();
            dlgRename.DataContext = new RenameGpxFilesViewModel(gpxFiles);
            dlgRename.Owner = _owner;
            dlgRename.ShowDialog();
        }
    }
}
