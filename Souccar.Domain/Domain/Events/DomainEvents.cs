using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Souccar.Domain.Events
{
    public class DomainEvents<TEvent> where TEvent : IDomainEvent
    {
        [ThreadStatic] private static List<Action<TEvent>> _actions;

        public static IServiceLocator ServiceLocator { get; set; }

        public static void ClearCallbacks()
        {
            _actions = null;
        }

        public static void Raise(TEvent args)
        {
            if (ServiceLocator != null)
            {
                foreach (var handler in ServiceLocator.GetAllInstances<IHandles<TEvent>>())
                {
                    handler.Handle(args);
                }
            }

            if (_actions != null)
            {
                foreach (var action in _actions)
                {
                    action.Invoke(args);
                }
            }
        }

        public static IDisposable Register(Action<TEvent> callback)
        {
            if (_actions == null)
            {
                _actions = new List<Action<TEvent>>();
            }
            _actions.Add(callback);
            return new DomainEventRegistrationRemover(() => _actions.Remove(callback)
                );
        }
    }
}