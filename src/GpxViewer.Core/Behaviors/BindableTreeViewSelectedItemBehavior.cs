using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace GpxViewer.Core.Behaviors
{
    /// <summary>
    /// This behavior allows to bind to the SelectedItem of a TreeView.
    /// </summary>
    public class BindableTreeViewSelectedItemBehavior : Behavior<TreeView>
    {
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(
                "SelectedItem", typeof(object), 
                typeof(BindableTreeViewSelectedItemBehavior), 
                new UIPropertyMetadata(null, OnSelectedItemChanged));

        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is not BindableTreeViewSelectedItemBehavior senderBeh) { return; }

            if (e.OldValue != null)
            {
                var oldTreeItem = FindContainerForItem(senderBeh.AssociatedObject.ItemContainerGenerator, e.OldValue);
                if (oldTreeItem != null)
                {
                    oldTreeItem.IsSelected = false;
                }
            }

            if (e.NewValue != null)
            {
                var newTreeItem = FindContainerForItem(senderBeh.AssociatedObject.ItemContainerGenerator, e.NewValue);
                if (newTreeItem != null)
                {
                    newTreeItem.IsSelected = true;
                }
            }
        }

        private static TreeViewItem? FindContainerForItem(ItemContainerGenerator containerGen, object item)
        {
            for (var loop = 0; loop < containerGen.Items.Count; loop++)
            {
                var actContainer = containerGen.ContainerFromIndex(loop);
                if(actContainer is not TreeViewItem actTreeViewItem){ continue; }

                if (actTreeViewItem.DataContext == item) { return actTreeViewItem; }

                var resultInner = FindContainerForItem(actTreeViewItem.ItemContainerGenerator, item);
                if (resultInner != null) { return resultInner; }
            }
            return null;
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectedItemChanged += this.OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.SelectedItemChanged -= this.OnTreeViewSelectedItemChanged;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.SelectedItem = e.NewValue;
        }
    }
}
