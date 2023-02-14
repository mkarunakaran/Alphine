using Sumday.Domain.Abstractions;

namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public abstract class Masked : ValueObject
    {
        protected const char Mask = 'X';

        public abstract string ToMaskedString();
    }
}
