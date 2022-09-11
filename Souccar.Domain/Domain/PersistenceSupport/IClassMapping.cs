using System;
using System.Linq.Expressions;

namespace Souccar.Domain.PersistenceSupport
{
    public interface IClassMapping
    {
        string ColumnName<T>(Expression<Func<T, object>> property)where T : class, new();

        string ColumnName<T>(string name) where T : class, new();

        string ColumnName(Type type, string name);
 
        string TableName<T>() where T : class, new();
        string TableName(Type type);
    }
}
