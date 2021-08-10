using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace FirLib.Core.Infrastructure.Services
{
    public class FirLibServiceContainer
    {
        private ConcurrentDictionary<Type, object> _services;

        public FirLibServiceContainer()
        {
            _services = new ConcurrentDictionary<Type, object>();
        }

        public void Register<TServiceType>(TServiceType serviceInstance)
            where TServiceType : class
        {
            this.Register(typeof(TServiceType), serviceInstance);
        }

        public void Register(Type serviceType, object serviceInstance)
        {
            EnsureInterfaceType(serviceType);

            if (_services.ContainsKey(serviceType))
            {
                throw new FirLibException($"A service of type {serviceType.FullName} is already registered!");
            }

            _services[serviceType] = serviceInstance;
        }

        public TServiceType GetService<TServiceType>()
            where TServiceType : class
        {
            var result = this.TryGetService<TServiceType>();
            if(result == null)
            {
                throw new FirLibException($"Unable to find service of type {typeof(TServiceType).FullName}!");
            }
            return result;
        }

        public TServiceType? TryGetService<TServiceType>()
            where TServiceType : class
        {
            var serviceType = typeof(TServiceType);
            EnsureInterfaceType(serviceType);

            if (_services.TryGetValue(serviceType, out var serviceInstance))
            {
                return serviceInstance as TServiceType;
            }
            return null;
        }

        /// <summary>
        /// Enumerates over all registered services.
        /// </summary>
        public IEnumerable<(Type, object)> GetAllServices()
        {
            foreach (var actPair in _services)
            {
                yield return (actPair.Key, actPair.Value);
            }
        }

        private static void EnsureInterfaceType(Type type)
        {
            if (!type.IsInterface)
            {
                throw new FirLibException($"Provided service type must be an interface!");
            }
        }
            
    }
}
