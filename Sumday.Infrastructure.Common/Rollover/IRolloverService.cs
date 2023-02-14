using Sumday.BoundedContext.SharedKernel.ValueObjects;
using Sumday.Infrastructure.Common.Http;

namespace Sumday.Infrastructure.Common.Rollover
{
    public interface IRolloverService : IHttpService
    {
        PlanId Plan { get; }
    }
}
