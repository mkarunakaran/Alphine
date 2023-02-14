namespace Sumday.Domain.Abstractions.EntryPorts
{
    public interface IOutputPort<TInteractorOutput>
    {
        void Output(UseCaseResult<TInteractorOutput> interactorOutput);
    }
}
