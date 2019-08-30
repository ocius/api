using Newtonsoft.Json;
using System;
using System.Xml;

namespace XmlToJson
{
    public static class Json
    {
        public static string FromXml(string xml)
        {
            var doc = new XmlDocument { XmlResolver = null }; //Setting resolver to null prevents XXE injection
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }
}
