using System;

namespace Valitru.Interfaces
{
    public interface IConditionalValidationRule<T> : IValidationRule<T>
    {
        Func<T, bool> ValidationCondition { get; set; }
    }
}