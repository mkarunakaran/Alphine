namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class Failure : UseCaseExecutionResult
    {
        public override bool IsSuccess => false;
    }
}
