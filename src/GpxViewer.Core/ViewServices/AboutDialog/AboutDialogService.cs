using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.Core.ViewServices.AboutDialog
{
    public class AboutDialogService : ViewServiceBase, IAboutDialogService
    {
        private Window _ownerWindow;
        private AboutDialogWindow? _currentDialog = null;

        public AboutDialogService(Window ownerWindow)
        {
            _ownerWindow = ownerWindow;
        }

        /// <inheritdoc />
        public Task ShowAboutDialogAsync()
        {
            var taskComplSource = new TaskCompletionSource<object?>(
                TaskCreationOptions.RunContinuationsAsynchronously);

            if (_currentDialog != null)
            { 
                // Bring current dialog to front
                if (_currentDialog.WindowState == WindowState.Minimized)
                {
                    _currentDialog.WindowState = WindowState.Normal;
                }
                _currentDialog.Activate();
            }
            else
            {
                // Show new dialog
                _currentDialog = new AboutDialogWindow();
                if (_ownerWindow != null)
                {
                    _currentDialog.Owner = _ownerWindow;
                }

                _currentDialog.Closed += (sender, eArgs) =>
                {
                    _currentDialog = null;
                    taskComplSource.TrySetResult(null);
                };
                _currentDialog.Show();
            }

            return taskComplSource.Task;
        }
    }
}
