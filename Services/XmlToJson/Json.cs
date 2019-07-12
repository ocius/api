using Newtonsoft.Json;
using System.Xml;

namespace XmlToJson
{
    public static class Json
    {
        public static string FromXml(string xml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }
}
