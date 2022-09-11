namespace Souccar.Domain.Validation
{
    public class BrokenBusinessRule
    {
        public BrokenBusinessRule(string property, string rule)
        {
            Property = property;
            Rule = rule;
        }

        
        public string Property { get; set; }

        public string Rule { get; set; }
    }
}