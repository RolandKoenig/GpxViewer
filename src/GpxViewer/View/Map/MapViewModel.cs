using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.View.Map
{
    public class MapViewModel : ViewModelBase
    {
        public static readonly MapViewModel DesignTimeData = new MapViewModel();

        public ObservableCollection<GpxFileViewModel> VisibleGpxFiles { get; }
            = new ObservableCollection<GpxFileViewModel>();

        public void AddGpxFile(GpxFileViewModel file)
        {
            this.VisibleGpxFiles.Add(file);
        }

        public void RemoveGpxFile(GpxFileViewModel file)
        {
            for(var loop=this.VisibleGpxFiles.Count - 1; loop >= 0; loop--)
            {
                if (this.VisibleGpxFiles[loop] == file)
                {
                    this.VisibleGpxFiles.RemoveAt(loop);
                }
            }
        }

        public bool ContainsGpxFile(GpxFileViewModel file)
        {
            foreach (var actGpxFile in this.VisibleGpxFiles)
            {
                if (actGpxFile == file)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
