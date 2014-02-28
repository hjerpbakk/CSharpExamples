using System;
using System.Collections.Generic;

namespace Infrastructure.IoC
{
    public class SimpleContainer : ISimpleContainer
    {
        private readonly Dictionary<Type, Func<object>> m_services;

        public SimpleContainer()
        {
            m_services = new Dictionary<Type, Func<object>>();
        }

        public void Register<TInterface, TClass>() where TClass : TInterface, new()
        {
            var type = typeof(TInterface);
            Func<object> ctor = () => new TClass();
            if (m_services.ContainsKey(type))
            {
                m_services[type] = ctor;
                return;
            }

            m_services.Add(type, ctor);
        }

        public TInterface Resolve<TInterface>() where TInterface : class
        {
            return (TInterface)m_services[typeof(TInterface)]();
        }
    }
}
