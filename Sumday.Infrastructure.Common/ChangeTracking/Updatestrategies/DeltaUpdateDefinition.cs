using System;
using System.Collections.Generic;
using System.Linq;

namespace Sumday.Infrastructure.Common.ChangeTracking.Updatestrategies
{
    public class DeltaUpdateDefinition : UpdateDefinition
    {
        private readonly Dictionary<string, ElementUpdate> elementsToReplace = new Dictionary<string, ElementUpdate>();

        public IReadOnlyCollection<ElementUpdate> ElementsToReplace => Array.AsReadOnly(this.elementsToReplace.Values.ToArray());

        public void Set(string elementName, object value)
        {
            this.elementsToReplace.Add(elementName, new ElementUpdate(elementName, value));
        }

        public void Merge(string elementNamePrefix, DeltaUpdateDefinition deltaUpdateDefinition)
        {
            foreach (var elementUpdate in deltaUpdateDefinition.ElementsToReplace)
            {
                var newElementName = GetElementNameWithPrefix(elementNamePrefix, elementUpdate.ElementName);
                this.Set(newElementName, elementUpdate.NewValue);
            }
        }

        private static string GetElementNameWithPrefix(string prefix, string originalName)
        {
            return prefix + "." + originalName;
        }
    }
}
