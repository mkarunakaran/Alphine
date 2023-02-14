namespace Sumday.Infrastructure.Common
{
    public interface ICallContext
    {
        object this[string index]
        {
                get;
                set;
        }

        bool ContainsKey(string index);
    }
}
