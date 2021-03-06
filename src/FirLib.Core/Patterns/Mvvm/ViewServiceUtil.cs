using System;
using System.Collections.Generic;
using System.Text;

namespace FirLib.Core.Patterns.Mvvm
{
    public static class ViewServiceUtil
    {
        public static T FindViewService<T>(this IViewServiceHost thisControl)
            where T : class
        {
            return (T)TryFindViewService(thisControl, typeof(T))!; 
        }

        public static T? TryFindViewService<T>(this IViewServiceHost thisControl)
            where T : class
        {
            return TryFindViewService(thisControl, typeof(T)) as T;
        }

        public static object? TryFindViewService(this IViewServiceHost thisControl, Type viewServiceType)
        {
            var actParent = thisControl;
            object? result = null;
            while (actParent != null)
            {
                foreach (var actViewService in actParent.ViewServices)
                {
                    if (actViewService == null) { continue; }

                    // ReSharper disable once UseMethodIsInstanceOfType
                    if (!viewServiceType.IsAssignableFrom(actViewService.GetType())) { continue; }

                    result = actViewService;
                }

                actParent = actParent.ParentViewServiceHost;
            }
            return result;
        }
    }
}
