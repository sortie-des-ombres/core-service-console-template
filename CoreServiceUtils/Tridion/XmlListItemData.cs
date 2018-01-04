using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CoreServiceUtils.Tridion
{
    [Serializable()]
    [XmlRoot("Item", Namespace = "http://www.tridion.com/ContentManager/5.0")]
    public class XmlListItemData
    {
        [XmlAttribute("ID")]
        public string Id { get; set; }

        [XmlAttribute("Title")]
        public string Title { get; set; }

        public static List<XmlListItemData> GetListOf(XElement element)
        {
            var serializer = new XmlSerializer(typeof(XmlListItemData));

            List<XmlListItemData> results = new List<XmlListItemData>();

            if(element.Nodes().Count() > 0)
            {
                var list = from n in element.Nodes()
                           select (XmlListItemData)serializer.Deserialize(n.CreateReader());

                if (list.Count() > 0)
                {
                    results = list.ToList<XmlListItemData>();
                }
            }

            return results;
        }
    }
}