using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FirLib.Core.Dialogs;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.ErrorAnalysis;

namespace FirLib.Core
{
    public static class FirLibExtensionsWpf
    {
        public static FirLibApplicationLoader AttachToWpfEnvironment(this FirLibApplicationLoader loader)
        {
            loader.AddStartupAction(() =>
            {
                Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;
            });

            return loader;
        }

        private static void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var exceptionInfo = new ExceptionInfo(e.Exception);

            var dlgError = new ErrorDialog();
            dlgError.DataContext = new ErrorDialogViewModel(exceptionInfo);
            dlgError.Owner = Application.Current.MainWindow;

            dlgError.ShowDialog();

            e.Handled = true;
        }
    }
}
