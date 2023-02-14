using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common.ChangeTracking;

namespace Sumday.Infrastructure.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dictionary<Type, BaseRepository> repositories = new Dictionary<Type, BaseRepository>();
        private bool hasCommitted;

        public UnitOfWork(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IRepository<TAggregate> Repository<TAggregate>()
            where TAggregate : AggregateRoot
        {
            if (this.repositories.ContainsKey(typeof(TAggregate)))
            {
                return (IRepository<TAggregate>)this.repositories[typeof(TAggregate)];
            }

            var repository = this.serviceProvider.GetRequiredService<IRepository<TAggregate>>();
            this.repositories.Add(typeof(TAggregate), (BaseRepository)repository);
            return repository;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            if (this.hasCommitted)
            {
                throw new InvalidOperationException("A UnitOfWork can only be committed a single time");
            }

            try
            {
                var preparedChanges = new Dictionary<BaseRepository, BasePreparedChangeModel>();

                foreach (var repository in this.repositories.Values)
                {
                    preparedChanges.Add(repository, repository.PrepareChangesForWrite());
                }

                foreach (var repository in this.repositories.Values)
                {
                    await repository.CommitChanges(preparedChanges[repository], cancellationToken);
                }

                this.hasCommitted = true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
