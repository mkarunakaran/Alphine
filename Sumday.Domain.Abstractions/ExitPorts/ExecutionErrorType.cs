namespace Sumday.Domain.Abstractions.ExitPorts
{
    public sealed class ExecutionErrorType : Enumeration<ExecutionErrorType, int>
    {
        public ExecutionErrorType(int type, string value)
         : base(type, value)
        {
        }

        public static ExecutionErrorType SystemValidation => new ExecutionErrorType(0, "SystemValidation");

        public static ExecutionErrorType DomainValidation => new ExecutionErrorType(1, "DomainValidation");

        public static ExecutionErrorType SystemSupport => new ExecutionErrorType(2, "SystemSupport");

        public static ExecutionErrorType General => new ExecutionErrorType(3, "General");

        public static ExecutionErrorType NotFound => new ExecutionErrorType(4, "NotFound");

        public static implicit operator ExecutionErrorType(int value) => FromValue(value);

        public static implicit operator int(ExecutionErrorType executionErrorType) => executionErrorType.Value;
    }
}
