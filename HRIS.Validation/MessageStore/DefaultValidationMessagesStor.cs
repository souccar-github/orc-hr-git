using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using HRIS.Validation.MessageKeys;
using Souccar.Domain.Validation;
using Souccar.Infrastructure.Services.Sys;
using SpecExpress.MessageStore;
using Souccar.Infrastructure.Core;

namespace HRIS.Validation.MessageStore
{
    public class DefaultValidationMessagesStor : IMessageStore
    {
        #region IMessageStore Members

        public string GetMessageTemplate(string key)
        {
            AddGroupToPreDefinedKeys(ref key);


#warning Mohammad alsaadi: this condition must be removed
            if (String.IsNullOrEmpty(ServiceFactory.LocalizationService.GetResource(key)))
            {
                return key + "_this key not added yet to store";
            }


            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentException("key is required", "key");
            }

            string messageTemplate = ServiceFactory.LocalizationService.GetResource(key);

            if (System.String.IsNullOrEmpty(messageTemplate))
            {
                throw new InvalidOperationException(
                    System.String.Format("Unable to find error message for {0} in resources file.", key));
            }

            return messageTemplate;
        }

        /// <Author>
        /// Muhammad Alsaadi
        /// </Author>
        /// 
        /// <summary>
        /// temp changes: this method always reterns true for development setuation
        /// this wil be altered later to original state
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        public bool IsMessageInStore(string key)
        {
#warning remember to delete return true
            return true;
            AddGroupToPreDefinedKeys(ref key);
            return !String.IsNullOrEmpty(ServiceFactory.LocalizationService.GetResource(key));
        }

        /// <Author>
        /// Muhammad Alsaadi
        /// </Author>
        /// 
        /// <summary>
        /// In any specification file we pass key exeplicitly for custom validation rules
        /// but spec express also bass key internaly which is not matches with database keys (database keys: Group+Key)
        /// </summary>
        private void AddGroupToPreDefinedKeys(ref string key)
        {
            PreDefinedValidatorKeys validatorKey;
            var isPreDefinedKey = Enum.TryParse(key, out validatorKey);
            if (isPreDefinedKey)
            {
                key = PreDefinedMessageKeysSpecExpress.ResourceGroupName + "_" + key;
            }
        }
        #endregion
    }
}
