using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.Modules.Map.Views
{
    public class MapViewModel : ViewModelBase
    {
        private string? _message;
        public string? Message
        {
            get { return _message; }
            set { this.SetProperty(ref _message, value); }
        }

        public MapViewModel()
        {
            this.Message = "View A from your Prism Module";
        }
    }
}
