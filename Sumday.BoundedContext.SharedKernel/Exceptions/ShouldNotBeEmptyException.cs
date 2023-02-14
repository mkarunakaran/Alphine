namespace Sumday.BoundedContext.SharedKernel.Exceptions
{
    public sealed class ShouldNotBeEmptyException : DomainException
    {
        private const string Required = "We need to know the {0} to move on.";

        internal ShouldNotBeEmptyException(string fieldName, string message = null)
            : base(fieldName, message)
        {
        }

        public override string Message
        {
            get
            {
                return string.IsNullOrEmpty(base.Message) ? string.Format(Required, this.FieldName) : base.Message;
            }
        }
    }
}
