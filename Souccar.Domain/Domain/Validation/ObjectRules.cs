#region

using System.Collections.Generic;

#endregion

namespace Souccar.Domain.Validation
{
    public abstract class ObjectRules<T> : IError<T>
    {
        private readonly List<BrokenBusinessRule> _rules;

        protected ObjectRules()
        {
            _rules = new List<BrokenBusinessRule>();
        }

        #region IError<T> Members

        public List<BrokenBusinessRule> GetBrokenRules(T t)
        {
            CheckRules(t);

            return _rules;
        }

        public List<BrokenBusinessRule> GetExpiredRules(IList<T> list)
        {
            foreach (T variable in list)
            {
                CheckExpiredRules(variable);
            }

            return _rules;
        }

        public void AddBrokenRule(BrokenBusinessRule brokenBusinessRule)
        {
            brokenBusinessRule.Rule = brokenBusinessRule.Rule.ToLowerInvariant();
            _rules.Add(brokenBusinessRule);
        }

        public abstract void CheckRules(T t);

        public virtual void CheckExpiredRules(T t)
        {
            
        }

        #endregion
    }
}