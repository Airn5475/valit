using System;
using Valitru.Interfaces;

namespace Valitru
{
    public class ConditionalValidationRule
    {
        public static ConditionalValidationRule<T> NewRule<T>()
        {
            return new ConditionalValidationRule<T>();
        }
    }

    public class ConditionalValidationRule<T> : ValidationRule<T>, IConditionalValidationRule<T>
    {
        public Func<T, bool> ValidationCondition { get; set; } = T => true;

        public override ValidationRuleResult Validate(T instance)
        {
            return !ValidationCondition(instance) 
                ? ValidationRuleResult.ValidationNotApplicableResult() 
                : base.Validate(instance);
        }

        public ValidationRule<T> OnlyCheckIf(Func<T, bool> validationConditionFunction)
        {
            if (validationConditionFunction == null) { throw new ArgumentNullException(nameof(validationConditionFunction)); }

            ValidationCondition = validationConditionFunction;

            return this;
        }
    }
}