using System.Windows.Controls;
using System.Windows.Input;
using FirLib.Core.Patterns.Mvvm;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal partial class FileTreeView : MvvmUserControl
    {
        public FileTreeView()
        {
            this.InitializeComponent();
        }

        private void OnTreeViewItem_DoubleClick(object sender, MouseButtonEventArgs args)
        {
            if (sender is not TreeViewItem treeViewItem) { return; }
            args.Handled = true;

            if (!treeViewItem.IsSelected) { return; }
            if (treeViewItem.DataContext is not FileTreeNodeViewModel fileNodeVM) { return; }

            if(this.DataContext is FileTreeViewModel viewModel)
            {
                viewModel.NotifyFileTreeNodeDoubleClick(fileNodeVM);
            }
        }

        private void OnTreView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is not TreeView treeView) { return; }
            if (!(treeView.IsKeyboardFocusWithin || treeView.IsKeyboardFocused)) { return; }
            if (this.DataContext is not FileTreeViewModel viewModel) { return; }

            switch (e.Key)
            {
                case Key.Up:
                    viewModel.TryNavigateUp();
                    e.Handled = true;
                    break;

                case Key.Down:
                    viewModel.TryNavigateDown();
                    e.Handled = true;
                    break;

                case Key.Left:
                    viewModel.TryCollapseSelectedNode();
                    e.Handled = true;
                    break;

                case Key.Right:
                    viewModel.TryExpandSelectedNode();
                    break;
            }
        }
    }
}
