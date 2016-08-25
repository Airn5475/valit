using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Valit.Interfaces;

namespace Valit.Helpers
{
    public static class ValidationRuleExtensions
    {
        public static ValidationRuleResult Validate<T>(this IEnumerable<IValidationRule<T>> validationRules, T instance)
        {
            var validationResults = new List<ValidationResult>();
            
            foreach (var vr in validationRules)
            {
                validationResults.AddRange(vr.Validate(instance).ValidationResults);
            }

            var res = new ValidationRuleResult
            {
                ValidationResults = validationResults,
                IsValid = !validationResults.Any()
            };
            return res;
        }

        public static IEnumerable<IConditionalValidationRule<T>> FilterRulesByTheirCondition<T>(this IEnumerable<IConditionalValidationRule<T>> rules, T instance)
        {
            return rules.Where(r => r.ValidationCondition(instance));
        }
    }
}