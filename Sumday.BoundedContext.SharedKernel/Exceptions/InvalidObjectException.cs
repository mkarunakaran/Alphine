namespace Sumday.BoundedContext.SharedKernel.Exceptions
{
    public sealed class InvalidObjectException : DomainException
    {
        public InvalidObjectException(string fieldName, string message = "This doesn't look right. Check your format.")
            : base(fieldName, message)
        {
        }
    }
}
