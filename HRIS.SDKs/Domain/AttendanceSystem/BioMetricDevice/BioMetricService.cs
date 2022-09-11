using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace HRIS.SDKs.Domain.AttendanceSystem.BioMetricDevice
{
    public static class BioMetricService
    {
        private static WindsorContainer _container;
        private static readonly List<string> SupportedDevicesKeys = new List<string>();

        public static void Initialize(IList<Assembly> devicesAssemblies)
        {
            _container = new WindsorContainer();

            foreach (var devicesAssembly in devicesAssemblies)
            {
                foreach (var type in devicesAssembly.GetTypes())
                {
                    if (typeof(IBioMetricDevice).IsAssignableFrom(type) && type.IsClass)
                    {
                        SupportedDevicesKeys.Add(type.FullName);
                        _container.Register(Component.For<IBioMetricDevice>().Named(type.FullName).ImplementedBy(type));
                    }
                }
            }
        }

        public static IBioMetricDevice GetDevice(string supportedDevicesKey)
        {
            if (_container == null)
            {
                throw new Exception("BioMetricFacrtory not initialized");
            }
            var device = _container.Resolve<IBioMetricDevice>(supportedDevicesKey);
            return device;
        }

        public static List<string> GetSupportedDevicesKeys()
        {
            if (_container == null)
            {
                throw new Exception("BioMetricFacrtory not initialized");
            }
            return SupportedDevicesKeys;
        }
    }
}
