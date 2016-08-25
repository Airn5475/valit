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
                validationResults.AddRange(vr.Validate(instance).ValidationResults);
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