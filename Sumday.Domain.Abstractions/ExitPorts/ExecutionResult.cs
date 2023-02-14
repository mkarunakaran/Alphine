using System.Collections.Generic;

namespace Sumday.Domain.Abstractions.ExitPorts
{
    public class ExecutionResult : IExecutionResult, IExecuteErrorNotifcations
    {
        private readonly List<ExecutionError> errors = new List<ExecutionError>();

        public bool Successfully
        {
            get { return this.errors.Count == 0; }
        }

        public IEnumerable<ExecutionError> Errors
        {
            get
            {
                foreach (var error in this.errors)
                {
                    yield return error;
                }
            }
        }

        public void AddError(ExecutionError error)
        {
            this.errors.Add(error);
        }

        public void RemoveError(ExecutionError error)
        {
            if (this.errors.Contains(error))
            {
                this.errors.Remove(error);
            }
        }
    }
}
