using System.Xml;
using System.Xml.Serialization;

namespace Sumday.Infrastructure.Common.Http
{
    [XmlRoot(Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Fault
    {
        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0, ElementName = "faultcode")]
        public XmlQualifiedName Faultcode { get; set; }

        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1, ElementName = "faultstring")]
        public string Faultstring { get; set; }

        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "anyURI", Order = 2, ElementName = "faultactor")]
        public string Faultactor { get; set; }

        [XmlElement(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3, ElementName ="detail")]
        public FaultDetail Detail { get; set; }
    }
}
