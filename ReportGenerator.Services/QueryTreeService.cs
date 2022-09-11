using System.Linq;
using System.Reflection;
using Souccar.ReportGenerator.Domain.QueryBuilder;
using System;
using System.Collections.Generic;
using System.Collections;

namespace Souccar.ReportGenerator.Services
{
    public class NHibernateQueryTreeService : IQueryTreeService
    {
        private readonly IQueryTreeParser _queryTreeParser;

        #region IQueryTreeService Members

        public NHibernateQueryTreeService(IQueryTreeParser queryTreeParser)
        {
            _queryTreeParser = queryTreeParser;
        }
        
        public object ExecuteQuery(QueryTree queryTree)
        {
            var result = _queryTreeParser.Parse(queryTree);
            if (result == null)
                return null;
            MethodInfo listMethod = result.GetType().GetMethods().Single(
                method => method.Name == "List" && method.IsGenericMethod).MakeGenericMethod(queryTree.Type);
            result = listMethod.Invoke(result, new object[0]);

            return result;
        }

        #endregion
    }
}