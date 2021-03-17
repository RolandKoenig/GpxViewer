using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using GpxViewer.View.Map;
using Microsoft.Xaml.Behaviors;

namespace GpxViewer.Utils.Behaviors
{
    public class ListBoxSelectionToViewModelBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            foreach (var actSelectedItem in this.AssociatedObject.SelectedItems)
            {
                if(!(actSelectedItem is GpxFileViewModel fileVM)){ continue; }
                fileVM.IsSelected = true;
            }

            this.AssociatedObject.SelectionChanged += OnListBox_SelectionChanged;
        }

        /// <inheritdoc />
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.SelectionChanged -= OnListBox_SelectionChanged;

            foreach(var actSelectedItem in this.AssociatedObject.SelectedItems)
            {
                if(!(actSelectedItem is GpxFileViewModel fileVM)){ continue; }
                fileVM.IsSelected = false;
            }
        }

        private static void OnListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.AddedItems != null)
            {
                foreach(var actAddedItem in e.AddedItems)
                { 
                    if(!(actAddedItem is GpxFileViewModel fileVM)){ continue; }

                    fileVM.IsSelected = true;
                }
            }

            if(e.RemovedItems != null)
            {
                foreach(var actRemovedItem in e.RemovedItems)
                {
                    if(!(actRemovedItem is GpxFileViewModel fileVM)){ continue; }

                    fileVM.IsSelected = false;
                }
            }
        }
    }
}
