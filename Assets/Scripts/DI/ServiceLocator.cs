using System;
using System.Collections.Generic;
using UnityEngine;

namespace DI
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

        public static void Register<T>(T service) where T : class
        {
            var type = typeof(T);

            if (!_services.TryAdd(type, service))
            {
                Debug.LogWarning($"Service {type} already registered, overriding.");
                _services[type] = service;
            }
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_services.TryGetValue(type, out var service))
                return service as T;

            Debug.LogError($"Service {type} not found! Did you forget to register it?");
            return null;
        }

        public static void Clear()
        {
            _services.Clear();
        }
    }
}