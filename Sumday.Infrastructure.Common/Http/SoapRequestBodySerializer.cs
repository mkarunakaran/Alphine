using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.Http
{
    public class SoapRequestBodySerializer : IRequestBodySerializer
    {
        private readonly Func<string, string> constructSoapRequest;

        public SoapRequestBodySerializer(Func<string, string> constructSoapRequest)
        {
            this.constructSoapRequest = constructSoapRequest;
        }

        public string SerializeBody<T>(T body)
        {
            if (body == null)
            {
                return null;
            }

            var stringWriter = new StringWriter();
            using XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true });
            var typeNamespace = typeof(T).GetRootNodeElementNameSpaceForType();
            var surpasNamepsaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName(string.Empty, typeNamespace) });
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, body, surpasNamepsaces);
            var soapRequest = this.constructSoapRequest(stringWriter.ToString());
            XmlDocument soapRequestDoc = new XmlDocument();
            soapRequestDoc.LoadXml(soapRequest);
            soapRequestDoc.PreserveWhitespace = false;
            return soapRequestDoc.OuterXml;
        }
    }
}
