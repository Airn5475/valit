using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Valitru.Interfaces;
using Valitru.Rules;

namespace Valitru.Helpers
{
    public static class ValidationRuleExtensions
    {
        public static ValidationRuleResult Validate<T>(this IEnumerable<IValidationRule<T>> validationRules, T instance)
        {
            var validationResults = new List<ValidationResult>();

            foreach (var vr in validationRules)
            {
                if (vr is StopProcessingIfInvalidCheckpoint<T> && validationResults.Any()) { break; }
                var result = vr.Validate(instance);
                if (result.IsValid) { continue; }
                validationResults.AddRange(result.ValidationResults);
                var stopProcessing = vr as IStopProcessing;
                if (stopProcessing != null && stopProcessing.StopProcessingIfInvalid) { break; }
            }

            var res = new ValidationRuleResult
            {
                ValidationResults = validationResults,
                IsValid = !validationResults.Any()
            };
            return res;
        }

        public static IEnumerable<ValidationRule<T>> FilterRulesByTheirCondition<T>(this IEnumerable<ValidationRule<T>> rules, T instance)
        {
            return rules.Where(r => r.IsApplicable(instance));
        }
    }
}