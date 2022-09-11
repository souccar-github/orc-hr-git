namespace Souccar.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;

    using Souccar.Domain.PersistenceSupport;

    public abstract class NHibernateQuery
    {
        protected virtual ISession Session
        {
            get
            {
                string factoryKey = SessionFactoryKeyHelper.GetKeyFrom(this);
                return NHibernateSession.CurrentFor(factoryKey);
            }
        }
    }
}