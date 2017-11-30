using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaKro.Libraries.RestConsumer.Implementation.IOC
{
    /// <summary>
    /// IocContainer implementation of IContainer.
    /// </summary>
    public class IocContainer : IContainer
    {
        /// <summary>
        /// Key: object containing the type of the object to resolve and the name of the instance (if any);
        /// Value: delegate that creates the instance of the object
        /// </summary>
        private readonly Dictionary<MappingKey, Func<object>> mappings;

        /// <summary>
        /// Creates a new instance of <see cref="IocContainer"/>
        /// </summary>
        public IocContainer()
        {
            mappings = new Dictionary<MappingKey, Func<object>>();
        }

        public bool IsRegistered(Type type, string instanceName = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            var key = new MappingKey(type, instanceName);
            return mappings.ContainsKey(key);
        }

        public bool IsRegistered<T>(string instanceName = null)
        {
            return IsRegistered(typeof(T), instanceName);
        }

        public void Register(Type type, Func<object> createInstanceDelegate, string instanceName = null)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (createInstanceDelegate == null)
                throw new ArgumentNullException("createInstanceDelegate");

            var key = new MappingKey(type, instanceName);

            if (mappings.ContainsKey(key))
            {
                const string errorMessageFormat = "The requested mapping already exists - {0}";
                throw new InvalidOperationException(string.Format(errorMessageFormat, key.ToTraceString()));
            }

            mappings.Add(key, createInstanceDelegate);
        }

        public void Register(Type from, Type to, string instanceName = null)
        {
            if (to == null)
                throw new ArgumentNullException("to");

            if (!from.IsAssignableFrom(to))
            {
                string errorMessage = string.Format("Error trying to register the instance: '{0}' is not assignable from '{1}'",
                    from.FullName, to.FullName);

                throw new InvalidOperationException(errorMessage);
            }

            Func<object> createInstanceDelegate = () => Activator.CreateInstance(to);
            Register(from, createInstanceDelegate, instanceName);
        }

        public void Register<T>(Func<T> createInstanceDelegate, string instanceName = null)
        {
            if (createInstanceDelegate == null)
                throw new ArgumentNullException("createInstanceDelegate");

            Func<object> createInstance = createInstanceDelegate as Func<object>;
            Register(typeof(T), createInstance, instanceName);
        }

        public void Register<TFrom, TTo>(string instanceName = null) where TTo : TFrom
        {
            Register(typeof(TFrom), typeof(TTo), instanceName);
        }

        public List<Type> ResolveAllComponentTypes()
        {
            List<Type> aoTypeList = new List<Type>();
            foreach (var mapping in mappings)
                aoTypeList.Add(mapping.Key.Type);
            return aoTypeList;
        }

        public object Resolve(Type type, string instanceName = null)
        {
            var key = new MappingKey(type, instanceName);
            Func<object> createInstance;

            if (mappings.TryGetValue(key, out createInstance))
            {
                var instance = createInstance();
                return instance;
            }

            return null;
        }

        public T Resolve<T>(string instanceName = null)
        {
            object instance = Resolve(typeof(T), instanceName);

            return (T)instance;
        }
    }
}
