using Valitru.Interfaces;

namespace Valitru.Rules
{
    public sealed class StopProcessingIfInvalidCheckpoint<T> : IValidationRule<T>, IStopProcessing
    {
        public bool StopProcessingIfInvalid => true;

        public ValidationRuleResult Validate(T instance) => ValidationRuleResult.ValidationPassedResult();
    }
}
