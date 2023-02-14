using Sumday.Domain.Abstractions.ExitPorts;
using System.Collections.Generic;

namespace Sumday.Domain.Abstractions
{
    public class ExecutionError : ValueObject
    {
        public ExecutionError(string message, string property, ExecutionErrorType executionErrorType, string resourceId = null)
        {
            this.Message = message;
            this.Property = property;
            this.ResourceId = resourceId;
            this.ExecutionErrorType = executionErrorType;
        }

        public string Message { get; }

        public string Property { get; }

        public string ResourceId { get; }

        public ExecutionErrorType ExecutionErrorType { get; }

        public override string ToString()
        {
            return string.Format("({0}) - {1}", this.Property, this.Message);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Property;
            yield return this.ResourceId;
            yield return this.ExecutionErrorType;
        }
    }
}
