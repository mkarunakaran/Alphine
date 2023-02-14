using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Sumday.Infrastructure.Extensions;

namespace Sumday.Infrastructure.Common.Http
{
    public class SoapResponseSerializer : IResponseDeserializer
    {
        private readonly Func<Fault, string, string, Task> faultHandler;

        public SoapResponseSerializer(Func<Fault, string, string, Task> faultHandler)
        {
            this.faultHandler = faultHandler;
        }

        public async Task<T> Deserialize<T>(string requestMessage, string responseMessage)
        {
            XDocument soap;
            try
            {
                soap = XDocument.Parse(responseMessage);
            }
            catch (Exception ex)
            {
                throw new SoapException("Soap Errror", "Exception in Response serialization", ex);
            }

            XNamespace ns = typeof(T).GetRootNodeElementNameSpaceForType();
            var result = soap.Descendants(ns + typeof(T).GetRootNodeElementNameForType()).FirstOrDefault();
            if (result != null)
            {
                // Consider caching generated XmlSerializers
                var serializer = new XmlSerializer(typeof(T));

                using var stringReader = new StringReader(result.ToString());
                return (T)serializer.Deserialize(stringReader);
            }

            XNamespace faultNs = "http://schemas.xmlsoap.org/soap/envelope/";
            var faultResult = soap.Descendants(faultNs + typeof(Fault).Name).FirstOrDefault();
            var faultSerializer = new XmlSerializer(typeof(Fault));
            if (faultResult != null)
            {
                using var faultReader = new StringReader(faultResult.ToString());
                var fault = (Fault)faultSerializer.Deserialize(faultReader);
                await this.faultHandler(fault, requestMessage, responseMessage);
            }

            return default;
        }
    }
}
