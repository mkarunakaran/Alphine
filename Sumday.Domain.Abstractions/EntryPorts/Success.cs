namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class Success : UseCaseExecutionResult
    {
        public override bool IsSuccess => true;
    }
}
