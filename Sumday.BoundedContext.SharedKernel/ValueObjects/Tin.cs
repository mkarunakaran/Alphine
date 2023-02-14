namespace Sumday.BoundedContext.SharedKernel.ValueObjects
{
    public abstract class Tin : Masked
    {
        public abstract TinType TinType { get; }

        public string Text { get; protected set; }

        public static implicit operator string(Tin tin)
        {
            return tin?.Text;
        }

        public override string ToString()
        {
            return this.Text.ToString();
        }
    }
}
