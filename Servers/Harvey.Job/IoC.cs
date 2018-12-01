using Autofac;
using System;

namespace Harvey.Job
{
    public static class IoC
    {
        private static IContainer _container;
        private static object _lock = new object();

        public static void SetContainer(IContainer container)
        {
            if(_container==null)
            {
                lock (_lock)
                {
                    if(_container==null)
                    {
                        _container = container;
                    }
                }
            }
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
