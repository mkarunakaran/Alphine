using System.Collections.Generic;
using System.Linq;

namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class UseCaseResult<TPayload>
    {
        private readonly List<ExecutionError> executionFailures = new List<ExecutionError>();

        public UseCaseResult(TPayload payload = default, List<ExecutionError> executionFailures = null, ResultCategory resultCategory = ResultCategory.Success)
        {
            this.Payload = payload;
            this.ResultCategory = resultCategory;
            this.executionFailures = executionFailures;
        }

        public TPayload Payload { get; }

        public bool IsSuccessful => this.ResultCategory == ResultCategory.Success;

        public string ErrorMessage { get; private set; }

        public ResultCategory ResultCategory { get; private set; }

        public IReadOnlyList<ExecutionError> ValidationFailures => this.executionFailures;

        public static UseCaseResult<TPayload> Success(TPayload payload) => new UseCaseResult<TPayload>(payload: payload);

        public static UseCaseResult<TPayload> NotFound(string error = null)
            => new UseCaseResult<TPayload>(resultCategory: ResultCategory.NotFound)
            {
                ErrorMessage = error
            };

        public static UseCaseResult<TPayload> AccessDenied(string error = null)
           => new UseCaseResult<TPayload>(resultCategory: ResultCategory.AccessDenied)
           {
               ErrorMessage = error
           };

        public static UseCaseResult<TPayload> ExecutionFail(IEnumerable<ExecutionError> executionFailures = null)
          => new UseCaseResult<TPayload>(executionFailures: executionFailures.ToList(), resultCategory: ResultCategory.ExecutionFailed);
    }
}
