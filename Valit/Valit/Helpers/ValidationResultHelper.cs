using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Valitru.Helpers
{
    public class ValidationResultHelper
    {
        public static IEnumerable<ValidationResult> NewResult(string errorMessage, IEnumerable<string> memberNames)
        {
            yield return new ValidationResult(errorMessage, memberNames);
        }

        public static IEnumerable<ValidationResult> NewResult(string errorMessage, string memberName)
        {
            return NewResult(errorMessage, new[] { errorMessage });
        }

        public static IEnumerable<ValidationResult> NewResult(string errorMessage)
        {
            yield return new ValidationResult(errorMessage);
        }

        public static IEnumerable<ValidationResult> GenericResult()
        {
            yield return new ValidationResult("Invalid data");
        }
    }
}