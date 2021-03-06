using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.View.Map;
using PropertyTools.Wpf;

namespace GpxViewer.ViewServices.Rename
{
    public class RenameGpxFilesViewModel : ValidatableViewModelBase
    {
        public static readonly RenameGpxFilesViewModel DesignTimeData =
            new RenameGpxFilesViewModel(new GpxFileViewModel[0]);

        private IEnumerable<GpxFileViewModel> _gpxFiles;

        [Required]
        public string SearchPattern { get; set; } = string.Empty;

        [Required]
        public string ReplaceBy { get; set; } = string.Empty;

        [Browsable(false)]
        public DelegateCommand Command_OK { get; }

        [Browsable(false)]
        public DelegateCommand Command_Cancel { get; }

        public RenameGpxFilesViewModel(IEnumerable<GpxFileViewModel> gpxFiles)
        {
            _gpxFiles = gpxFiles;

            this.Command_OK = new DelegateCommand(this.OnCommand_OK_Execute);
            this.Command_Cancel = new DelegateCommand(() => this.CloseWindow(null));
        }

        private void OnCommand_OK_Execute()
        {
            if (string.IsNullOrEmpty(this.SearchPattern)) { return; }

            foreach(var actGpxFile in _gpxFiles)
            {
                actGpxFile.Name = actGpxFile.Name.Replace(this.SearchPattern, this.ReplaceBy);
            }

            this.CloseWindow(null);
        }
    }
}
