#region

using System.Collections.Generic;
using NHibernate;

#endregion

namespace NHibernateDBGenerator.NHibernate.Helpers
{
    public interface ISessionFactoryProvider
    {
        IEnumerable<ISessionFactory> GetSessionFactories();
    }
}