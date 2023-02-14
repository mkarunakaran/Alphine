using Sumday.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.BoundedContext.SharedKernel.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException(string message)
            : this(message, Enumerable.Empty<ExecutionError>())
        {
        }

        public ValidationException(string message, IEnumerable<ExecutionError> errors)
            : base(message)
        {
            this.Errors = errors;
        }

        public ValidationException(IEnumerable<ExecutionError> errors)
            : base(BuildErrorMessage(errors))
        {
            this.Errors = errors;
        }

        public IEnumerable<ExecutionError> Errors { get; private set; }

        private static string BuildErrorMessage(IEnumerable<ExecutionError> errors)
        {
            var arr = errors.Select(x => $"{Environment.NewLine} -- {x.Property}: {x.Message}");
            return "Validation failed: " + string.Join(string.Empty, arr);
        }
    }
}
