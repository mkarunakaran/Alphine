namespace Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies
{
    public class ElementUpdate
    {
        public ElementUpdate(string elementName, object newValue)
        {
            this.ElementName = elementName;
            this.NewValue = newValue;
        }

        public string ElementName { get; }

        public object NewValue { get; }
    }
}
