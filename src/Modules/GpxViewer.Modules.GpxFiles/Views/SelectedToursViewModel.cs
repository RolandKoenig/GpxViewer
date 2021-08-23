﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GpxViewer.Core.Patterns;
using GpxViewer.Modules.GpxFiles.Interface.Messages;

namespace GpxViewer.Modules.GpxFiles.Views
{
    internal class SelectedToursViewModel : GpxViewerViewModelBase
    {
        public ObservableCollection<SelectedTourViewModel> SelectedTours { get; }

        public Visibility SelectedToursEditVisibility =>
            this.SelectedTours.Count == 1 ? Visibility.Visible : Visibility.Collapsed;

        public SelectedToursViewModel()
        {
            this.SelectedTours = new ObservableCollection<SelectedTourViewModel>();
        }

        private void OnMessageReceived(MessageGpxFileRepositoryNodeSelectionChanged message)
        {
            this.SelectedTours.Clear();

            if (message.SelectedNodes != null)
            {
                foreach(var actSelectedNode in message.SelectedNodes)
                {
                    foreach (var actTour in actSelectedNode.GetAllAssociatedTours())
                    {
                        this.SelectedTours.Add(new SelectedTourViewModel(actTour));
                    }
                }
            }

            this.RaisePropertyChanged(nameof(this.SelectedToursEditVisibility));
        }
    }
}