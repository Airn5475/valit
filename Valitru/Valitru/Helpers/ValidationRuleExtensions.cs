using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
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
            var counter = 0;

            foreach (var vr in validationRules)
            {
                counter++;
                if (vr is StopProcessingIfInvalidCheckpoint<T> && validationResults.Any())
                {
                    Trace.WriteLine($"Rule {counter}: StopProcessingIfInvalidCheckpoint - {validationResults.Count} Validation Results Exist - Validation Exited");
                    break;
                }
                var result = vr.Validate(instance);
                if (result.IsValid)
                {
                    Trace.WriteLine($"Rule {counter}: Valid");
                    continue;
                }
                if (result.NotApplicable)
                {
                    Trace.WriteLine($"Rule {counter}: NotApplicable - Validation Continued");
                    continue;
                }
                validationResults.AddRange(result.ValidationResults);
                var stopProcessing = vr as IStopProcessing;
                if (stopProcessing != null && stopProcessing.StopProcessingIfInvalid)
                {
                    Trace.WriteLine($"Rule {counter}: IStopProcessing - Validation Failed & Exited");
                    break;
                }
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