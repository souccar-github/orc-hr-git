using System;

namespace Souccar.Domain.Events
{
    public class DomainEventRegistrationRemover : IDisposable
    {
        private readonly Action _callOnDispose;

        public DomainEventRegistrationRemover(Action toCall)
        {
            _callOnDispose = toCall;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _callOnDispose.DynamicInvoke();
        }

        #endregion
    }
}