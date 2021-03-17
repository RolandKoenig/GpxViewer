using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using FirLib.Core.Dialogs;
using FirLib.Core.Infrastructure;
using FirLib.Core.Patterns.ErrorAnalysis;
using FirLib.Core.Patterns.Messaging;

namespace FirLib.Core
{
    public static class FirLibExtensionsWpf
    {
        public static FirLibApplicationLoader AttachToWpfEnvironment(this FirLibApplicationLoader loader)
        {
            Application.Current.DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;

            var uiMessenger = new FirLibMessenger();
            uiMessenger.ConnectToGlobalMessaging(
                FirLibMessengerThreadingBehavior.EnsureMainSyncContextOnSyncCalls,
                FirLibConstants.MESSENGER_NAME_GUI,
                SynchronizationContext.Current);

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
