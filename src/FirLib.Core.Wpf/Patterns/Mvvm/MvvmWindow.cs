using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FirLib.Core.ViewServices;

namespace FirLib.Core.Patterns.Mvvm
{
    public class MvvmWindow : Window, IViewServiceHost
    {
        private ViewServiceContainer _viewServiceContainer;

        /// <inheritdoc />
        public ICollection<IViewService> ViewServices => _viewServiceContainer.ViewServices;

        /// <inheritdoc />
        public IViewServiceHost? ParentViewServiceHost => null;

        public object? DialogResultMvvm { get; private set; }

        public MvvmWindow()
        {
            _viewServiceContainer = new ViewServiceContainer(this);

            this.DataContextChanged += this.OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(e.OldValue is ViewModelBase oldViewModel)
            {
                if (ReferenceEquals(oldViewModel.AssociatedView, this))
                {
                    oldViewModel.CloseWindowRequest -= this.OnViewModel_CloseWindowRequest;  
                    oldViewModel.ViewServiceRequest -= this.OnViewModel_ViewServiceRequest;
                    oldViewModel.AssociatedView = null;
                }
            }

            if(e.NewValue is ViewModelBase newViewModel)
            {
                if (newViewModel.AssociatedView == null)
                {
                    newViewModel.CloseWindowRequest += this.OnViewModel_CloseWindowRequest;
                    newViewModel.ViewServiceRequest += this.OnViewModel_ViewServiceRequest;
                    newViewModel.AssociatedView = this;
                }
            }
        }

        private void OnViewModel_ViewServiceRequest(object? sender, ViewServiceRequestEventArgs e)
        {
            var foundViewService = this.TryFindViewService(e.ViewServiceType);
            if(foundViewService != null)
            {
                e.ViewService = foundViewService;
            }
            else
            {
                e.ViewService = WpfDefaultViewServices.TryGetViewService(this, e.ViewServiceType);
            }
        }

        private void OnViewModel_CloseWindowRequest(object? sender, CloseWindowRequestEventArgs e)
        {
            this.DialogResultMvvm = e.DialogResult;
            this.Close();
        }
    }
}
