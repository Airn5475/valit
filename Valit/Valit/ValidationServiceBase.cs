namespace Valit
{
    public abstract class ValidationServiceBase<T>
    {
        public abstract ValidationRules<T> AllRules();

        public virtual ValidationRuleResult Validate(T instance)
        {
            return AllRules().Validate(instance);
        }
    }
}