namespace Souccar.Domain.Validation
{
    public interface IRule<T>
    {
        ObjectRules<T> Rules { get; }
    }
}