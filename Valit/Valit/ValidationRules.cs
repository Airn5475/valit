using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Valit.Helpers;
using Valit.Interfaces;

namespace Valit
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
