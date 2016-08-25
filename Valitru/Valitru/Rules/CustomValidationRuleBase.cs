using System;

namespace Valitru.Rules
{
    public abstract class CustomValidationRuleBase<T, TMember> : ConditionalValidationRule<T>
    {
        public Func<T, TMember> Member { get; set; }

        public ConditionalValidationRule<T> MemberToValidate(Func<T, TMember> property)
        {
            this.Member = property;
            return this;
        }
        
    }
}