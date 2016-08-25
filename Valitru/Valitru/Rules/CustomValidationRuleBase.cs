using System;

namespace Valitru.Rules
{
    public abstract class CustomValidationRuleBase<T, TMember> : ValidationRule<T>
    {
        protected Func<T, TMember> Member { get; set; }

        public ValidationRule<T> MemberToValidate(Func<T, TMember> member)
        {
            if (member == null) { throw new ArgumentNullException(nameof(member)); }
            Member = member;
            return this;
        }

    }
}