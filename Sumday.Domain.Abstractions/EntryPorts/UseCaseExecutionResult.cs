namespace Sumday.Domain.Abstractions.EntryPorts
{
    public abstract class UseCaseExecutionResult
    {
        public static UseCaseExecutionResult Success => new Success();

        public static UseCaseExecutionResult Failure => new Failure();

        public abstract bool IsSuccess { get; }
    }
}
