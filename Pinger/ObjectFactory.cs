using System;
using System.Threading;
using StructureMap;

namespace Pinger
{
    public static class ObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder =
            new Lazy<Container>(DefaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container => _containerBuilder.Value;

        private static Container DefaultContainer()
        {
            return new Container(x =>
            {
                x.AddRegistry(new PingerRegistry());
            });
        }
    }
}