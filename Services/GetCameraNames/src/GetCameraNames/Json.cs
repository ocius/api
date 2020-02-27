using Newtonsoft.Json;
using System.Xml;

namespace GetCameraNames
{
    public static class Json
    {
        public static string FromXml(string xml)
        {
            var doc = new XmlDocument(); //Setting resolver to null prevents XXE injection
            doc.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(doc);
        }
    }
}
