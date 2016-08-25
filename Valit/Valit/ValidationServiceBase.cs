namespace Valit
{
    public abstract class ValidationServiceBase<T>
    {
        protected abstract ValidationRules<T> AllRules();

        public virtual ValidationRuleResult Validate(T instance)
        {
            return AllRules().Validate(instance);
        }
    }
}