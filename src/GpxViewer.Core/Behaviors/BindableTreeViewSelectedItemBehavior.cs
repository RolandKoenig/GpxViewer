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

            if (senderBeh.AssociatedObject.ItemContainerGenerator.ContainerFromItem(e.NewValue) is TreeViewItem itemContainer)
            {
                itemContainer.IsSelected = true;
            }
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
