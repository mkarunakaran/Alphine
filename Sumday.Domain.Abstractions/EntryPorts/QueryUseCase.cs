namespace Sumday.Domain.Abstractions.EntryPorts
{
    public class QueryUseCase<TQuery, TPayLoad> : UseCase
        where TQuery : IQueryInputPort
    {
        public QueryUseCase(TQuery query, IQueryOutputPort<TPayLoad> outputPort)
        {
            this.Query = query;
            this.OutputPort = outputPort;
        }

        public IQueryOutputPort<TPayLoad> OutputPort { get; }

        public TQuery Query { get; }
    }
}
