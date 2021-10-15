using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core.Dialogs;
using FirLib.Core.Patterns.ErrorAnalysis;
using FirLib.Core.Patterns.Mvvm;

namespace FirLib.Core.ViewServices
{
    public class WpfErrorDialogService : ViewServiceBase, IErrorDialogService
    {
        private Window _owner;

        public WpfErrorDialogService(Window owner)
        {
            _owner = owner;
        }

        public Task ShowAsync(Exception errorDetails)
        {
            var dlgError = new ErrorDialog();
            dlgError.Owner = _owner;
            dlgError.DataContext = new ErrorDialogViewModel(new ExceptionInfo(errorDetails));
            dlgError.ShowDialog();

            return Task.CompletedTask;
        }
    }
}
