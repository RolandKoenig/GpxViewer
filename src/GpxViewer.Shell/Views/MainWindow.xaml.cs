using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.Mvvm;
using GpxViewer.Core.ViewServices.AboutDialog;

namespace GpxViewer.Shell.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvvmWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();

            if (FirLibApplication.IsLoaded)
            {
                // Register view services
                this.ViewServices.Add(new AboutDialogService(this));

                // Register events
                this.AllowDrop = true;
                this.DragOver += this.OnDragOver;
                this.Drop += this.OnDrop;
            }
        }

        /// <inheritdoc />
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.DataContext is MainWindowViewModel { ExitApproved: false } viewModel)
            {
                e.Cancel = true;

                this.Dispatcher.BeginInvoke(
                    new Action(() => viewModel.Command_Exit.Execute(null)),
                    DispatcherPriority.Normal);
            }
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            var viewModel = this.DataContext as MainWindowViewModel;
            if (viewModel == null) { e.Effects = DragDropEffects.None; }

            if ((e.Data.GetData(DataFormats.FileDrop, true) is not string[] fileDropItems) ||
                (string.IsNullOrEmpty(fileDropItems[0])))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            var viewModel = this.DataContext as MainWindowViewModel;
            if (viewModel == null) { return; }

            if ((e.Data.GetData(DataFormats.FileDrop, true) is not string[] fileDropItems) ||
                (string.IsNullOrEmpty(fileDropItems[0])))
            {
                return;
            }

            viewModel.NotifyOSFileDrop(fileDropItems);
        }
    }
}
