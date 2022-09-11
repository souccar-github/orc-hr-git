#region

using System.Collections.Generic;

#endregion

namespace Souccar.Domain.Validation
{
    public interface IError<T>
    {
        List<BrokenBusinessRule> GetBrokenRules(T t);
        List<BrokenBusinessRule> GetExpiredRules(IList<T> list);
        void AddBrokenRule(BrokenBusinessRule brokenBusinessRule);
        void CheckRules(T t);
        void CheckExpiredRules(T t);
    }
}