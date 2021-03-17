using Prism.Mvvm;

namespace GpxViewer.Shell.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Application";

        public string Title
        {
            get { return _title; }
            set { this.SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
