namespace Valit.Interfaces
{
    public interface IValidationRule<in T>
    {
        ValidationRuleResult Validate(T instance);
    }
}