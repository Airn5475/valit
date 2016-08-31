using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Valitru
{
    public class ValidationRuleResult
    {
        public bool NotApplicable { get; internal set; }
        public bool IsValid { get; set; }
        public IEnumerable<ValidationResult> ValidationResults { get; set; } = Enumerable.Empty<ValidationResult>();
        public static ValidationRuleResult ValidationNotApplicableResult() => new ValidationRuleResult() { NotApplicable = true };
        public static ValidationRuleResult ValidationPassedResult() => new ValidationRuleResult() { IsValid = true };
        public static ValidationRuleResult ValidationFailedResult() => new ValidationRuleResult() { IsValid = false };
    }
}
