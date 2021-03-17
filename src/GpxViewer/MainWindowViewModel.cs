using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirLib.Core.Patterns;
using FirLib.Core.Patterns.Mvvm;
using FirLib.Core.ViewServices;
using FirLib.Formats.Gpx;
using GpxViewer.View.Map;
using GpxViewer.ViewServices.Rename;
using GpxViewer.Utils;

namespace GpxViewer
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string _title;

        public ObservableCollection<GpxFileViewModel> LoadedGpxFiles { get; }
            = new ObservableCollection<GpxFileViewModel>();

        public MapViewModel MapVM { get; }

        public string Title
        {
            get => _title;
            private set
            {
                if (_title != value)
                {
                    _title = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand Command_OpenFile { get; }

        public DelegateCommand Command_Close { get; }

        public DelegateCommand Command_SaveChanges { get; }

        public DelegateCommand Command_Exit { get; }

        public DelegateCommand Command_Tools_RenameBySearchReplace { get; }

        public DelegateCommand Command_Tools_RenameToFileNames { get; }

        public MainWindowViewModel()
        {
            _title = string.Empty;
            this.MapVM = new MapViewModel();

            this.LoadedGpxFiles.CollectionChanged += this.OnLoadedGpxFiles_CollectionChanged;

            this.Command_OpenFile = new DelegateCommand(this.OnCommand_OpenFile_Execute);
            this.Command_Close = new DelegateCommand(
                this.OnCommand_Close_Execute,
                () => this.LoadedGpxFiles.Count > 0);
            this.Command_SaveChanges = new DelegateCommand(
                this.OnCommand_SaveChanges_Execute,
                () => this.LoadedGpxFiles.Any(file => file.HasChanged));
            this.Command_Exit = new DelegateCommand(this.OnCommand_Exit_Execute);

            this.Command_Tools_RenameBySearchReplace = new DelegateCommand(
                this.OnCommand_Tools_RenameBySearchReplace_Execute,
                () => this.LoadedGpxFiles.Count > 0);
            this.Command_Tools_RenameToFileNames = new DelegateCommand(
                this.OnCommand_Tools_RenameToFileNames_Execute,
                () => this.LoadedGpxFiles.Count > 0);

            this.UpdateEnabledState();
        }

        private void UpdateEnabledState()
        {
            this.Command_Close.RaiseCanExecuteChanged();
            this.Command_SaveChanges.RaiseCanExecuteChanged();
            this.Command_Tools_RenameBySearchReplace.RaiseCanExecuteChanged();

            var anythingChanged = this.LoadedGpxFiles.Any(file => file.HasChanged);

            if (anythingChanged) { this.Title = "RK Gpx Viewer*"; }
            else { this.Title = "RK Gpx Viewer"; }
        }

        private void OnLoadedGpxFiles_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach(GpxFileViewModel actOldGpxFile in e.OldItems)
                {
                    actOldGpxFile.PropertyChanged -= this.OnGpxFile_PropertyChanged;
                }
            }

            if (e.NewItems != null)
            {
                foreach(GpxFileViewModel actNewGpxFile in e.NewItems)
                {
                    actNewGpxFile.PropertyChanged += this.OnGpxFile_PropertyChanged;
                }
            }
        }

        private void OnGpxFile_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GpxFileViewModel.HasChanged))
            {
                this.UpdateEnabledState();
            }
        }

        private async void OnCommand_OpenFile_Execute()
        {
            var srvOpenFileDialog = this.GetViewService<IOpenFileViewService>();
            var fileNames = await srvOpenFileDialog.ShowOpenMultipleFilesDialogAsync(
                new[] {new FileDialogFilter("GPS Exchange Format", "gpx")},
                "Select .gpx file(s)");
            if ((fileNames != null) && (fileNames.Length > 0))
            {
                foreach (var actFileName in fileNames)
                {
                    var gpxFile = GpxFile.Deserialize(actFileName);

                    var newGpxVM = new GpxFileViewModel(actFileName, gpxFile, this.MapVM);
                    newGpxVM.IsVisible = true;
                    this.LoadedGpxFiles.Add(newGpxVM);
                }
            }

            this.UpdateEnabledState();
        }

        private void OnCommand_Close_Execute()
        {
            foreach(var actLoadedFile in this.LoadedGpxFiles)
            {
                actLoadedFile.IsVisible = false;
            }
            this.LoadedGpxFiles.Clear();

            this.UpdateEnabledState();
        }

        private void OnCommand_SaveChanges_Execute()
        {
            foreach (var actFile in this.LoadedGpxFiles)
            {
                if(!actFile.HasChanged){ continue; }

                actFile.SaveFile();
            }

            this.UpdateEnabledState();
        }

        private void OnCommand_Exit_Execute()
        {
            this.CloseWindow(true);
        }

        private void OnCommand_Tools_RenameBySearchReplace_Execute()
        {
            var srvRename = this.GetViewService<IRenameGpxFilesViewService>();
            srvRename.Rename(this.LoadedGpxFiles);

            this.UpdateEnabledState();
        }

        private void OnCommand_Tools_RenameToFileNames_Execute()
        {
            foreach (var actGpxFile in this.LoadedGpxFiles)
            {
                actGpxFile.Name = Path.GetFileName(actGpxFile.FilePath);
            }

            this.UpdateEnabledState();
        }
    }
}
