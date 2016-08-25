using System;

namespace Valitru.Rules.Library
{
    public class StringCannotBeNullOrWhitespaceRule<T> : CustomValidationRuleBase<T, string>
    {
        public override ValidationRuleResult Validate(T instance)
        {
            ValidIf(blah => !string.IsNullOrWhiteSpace(Member(instance)));
            return base.Validate(instance);
        }
    }
}