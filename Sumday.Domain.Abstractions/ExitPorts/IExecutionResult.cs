using System.Collections.Generic;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IExecutionResult
    {
        bool Successfully { get; }
    }
}
