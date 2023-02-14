using System.Threading;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IUnitOfWork
    {
        IRepository<TAggregate> Repository<TAggregate>()
            where TAggregate : AggregateRoot;

        Task SaveChanges(CancellationToken cancellationToken);
    }
}
