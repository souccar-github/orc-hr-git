using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;

namespace Souccar.NHibernate.Wcf
{
    public class ServiceLocatorInstanceProvider : IInstanceProvider
    {
        private Type type;

        public ServiceLocatorInstanceProvider(Type type)
        {
            // TODO: Complete member initialization
            this.type = type;
        }
        public object GetInstance(InstanceContext instanceContext)
        {
            throw new NotImplementedException();
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            throw new NotImplementedException();
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            throw new NotImplementedException();
        }
    }
}
