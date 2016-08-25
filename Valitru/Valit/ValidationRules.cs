using System.Collections.Generic;
using Valitru.Helpers;
using Valitru.Interfaces;

namespace Valitru
{
    public class ValidationRules<T> : IValidationRule<T>
    {
        public List<IValidationRule<T>> Rules { get; set; }
        
        public ValidationRules()
        {
            Rules = new List<IValidationRule<T>>();
        }

        public ValidationRuleResult Validate(T instance)
        {
            return Rules.Validate(instance);
        }

    }
}
