using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Souccar.Core;
using SpecExpress.MessageStore;

namespace HRIS.Validation.MessageStore
{
        public class CustomMessagesMessageStore : IMessageStore
        {
            private ResourceManager _resource;

            public CustomMessagesMessageStore(ResourceManager resourceManager)
            {
                _resource = resourceManager;
            }

            public CustomMessagesMessageStore()
            {
                _resource = CustomMessagesResource.ResourceManager;
            }

            #region IMessageStore Members
            
            public string GetMessageTemplate(string key)
            {
                if (String.IsNullOrEmpty(key))
                {
                    throw new ArgumentException("key is required", "key");
                }

                string messageTemplate = _resource.GetString(key);

                if (System.String.IsNullOrEmpty(messageTemplate))
                {
                    throw new InvalidOperationException(
                        System.String.Format("Unable to find error message for {0} in resources file.", key));
                }

                return messageTemplate;
            }

            public bool IsMessageInStore(string key)
            {
                return !String.IsNullOrEmpty(_resource.GetString(key));
            }

            #endregion
    }
}
