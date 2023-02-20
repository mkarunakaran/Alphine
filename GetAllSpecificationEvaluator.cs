using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Sumday.BoundedContext.SharedKernel.Exceptions;
using Sumday.Domain.Abstractions;
using Sumday.Domain.Abstractions.ExitPorts;
using Sumday.Infrastructure.Common;
using Sumday.Infrastructure.Extensions;
using ValidationException = Sumday.BoundedContext.SharedKernel.Exceptions.ValidationException;

namespace Sumday.Infrastructure.Surpas
{
    public class GetAllSpecificationEvaluator<TAggregate, TAllSpecification> : IGetAllSpecificationEvaluator<TAggregate, TAllSpecification>
         where TAggregate : AggregateRoot
         where TAllSpecification : IAllSpecification<TAggregate>
    {
        private readonly IServiceProvider serviceProvider;

        public GetAllSpecificationEvaluator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.Metadata[SurpasConstants.CurrentPage] = 1;
            this.Metadata[SurpasConstants.PageSzie] = 10;
            this.Metadata[SurpasConstants.TotalCount] = 100;
        }

        protected Dictionary<string, object> Metadata { get; set; } = new Dictionary<string, object>();

        public virtual async Task<IReadOnlyList<TAggregate>> GetAll(TAllSpecification specification, CancellationToken cancellationToken = default)
        {
            var aggregates = await this.GetPagedAggregates(specification, cancellationToken);
            return aggregates.AsReadOnly();
        }

        protected List<TAggregate> ApplyOperations(TAllSpecification specification, List<TAggregate> aggregates)
        {
            if (specification.OrderBy != null)
            {
                aggregates = aggregates.OrderBy(specification.OrderBy.Compile()).ToList();
            }
            else if (specification.OrderByDescending != null)
            {
                aggregates = aggregates.OrderBy(specification.OrderByDescending.Compile()).ToList();
            }

            var pagingEnabled = specification.IsPagingEnabled;
            var totalReccords = (int)this.Metadata[SurpasConstants.TotalCount];
            if (totalReccords != aggregates.Count && specification.IsPagingEnabled)
            {
                pagingEnabled = false;
            }

            if (pagingEnabled)
            {
                aggregates = aggregates.Skip(specification.Skip)
                             .Take(specification.Take)
                             .ToList();
            }

            this.Metadata[SurpasConstants.CurrentPage] = specification.Skip;
            this.Metadata[SurpasConstants.PageSzie] = specification.Take;
            this.Metadata[SurpasConstants.TotalCount] = aggregates.Count;
            return aggregates;
        }

        protected virtual async Task<List<TAggregate>> GetPagedAggregates(TAllSpecification specification, CancellationToken cancellationToken = default)
        {
            var baseSpecification = specification as BaseAllSpecification<TAggregate>;
            var aggregates = new List<TAggregate>();
            var aggregateService = this.serviceProvider.GetRequiredService<IAggregateService<TAggregate>>();
            try
            {
                aggregates = await aggregateService.GetAll(specification, this.Metadata, cancellationToken);
                this.Metadata[SurpasConstants.TotalCount] = aggregates.Count;
                if (baseSpecification.IsSatisfied.Successfully)
                {
                    aggregates = this.ApplyOperations(specification, aggregates);
                }
            }
            catch (ValidationException ex)
            {
                ex.Errors.ForEach(err => baseSpecification.Notification.AddError(err));
            }
            catch (DomainException ex)
            {
                baseSpecification.Notification.AddError(new ExecutionError(ex.Message, ex.FieldName, ExecutionErrorType.DomainValidation));
            }
            catch (SumdaySupportCodeException ex)
            {
                baseSpecification.Notification.AddError(new ExecutionError(ex.Message, ex.SupportCode.ToString(), ExecutionErrorType.SystemSupport));
            }
            catch (Exception ex)
            {
                baseSpecification.Notification.AddError(new ExecutionError(ex.Message, string.Empty, ExecutionErrorType.General));
            }

            return aggregates;
        }
    }
}
