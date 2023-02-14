using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public interface IExecuteErrorNotifcations
    {
        IEnumerable<ExecutionError> Errors { get; }
    }
}
