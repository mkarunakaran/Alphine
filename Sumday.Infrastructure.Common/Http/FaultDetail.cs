using System.Xml;
using System.Xml.Serialization;

namespace Sumday.Infrastructure.Common.Http
{
    [XmlType]
    public class FaultDetail
    {
        [XmlAnyElementAttribute(Order = 0)]
        public XmlElement[] Any { get; set; }
    }
}
