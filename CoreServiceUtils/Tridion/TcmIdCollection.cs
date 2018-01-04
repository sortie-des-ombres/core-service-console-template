using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Tridion
{

    public class TcmIdCollection
    {
        private Fields fields;
        private ItemFieldDefinitionData definition;

        public TcmIdCollection(Fields _fields, ItemFieldDefinitionData _definition)
        {
            fields = _fields;
            definition = _definition;
        }

        public int Count
        {
            get { return fields.GetFieldElements(definition).Count(); }
        }

        public bool IsLinkField
        {
            get { return definition is ComponentLinkFieldDefinitionData || definition is ExternalLinkFieldDefinitionData || definition is MultimediaLinkFieldDefinitionData || definition is KeywordFieldDefinitionData; }
        }
        public bool IsRichTextField
        {
            get { return definition is XhtmlFieldDefinitionData; }
        }

        public bool Contains(string value)
        {
            List<XmlElement> values = fields.GetFieldElements(definition).ToList<XmlElement>();
            bool success = false;

            var list = from v in values
                       where v.Attributes["xlink:href"].Value == value
                       select v;

            success = list.Count() > 0;

            return success;
        }

        public string this[int i]
        {
            get
            {
                XmlElement[] elements = fields.GetFieldElements(definition).ToArray();
                if (i >= elements.Length) throw new IndexOutOfRangeException();
                    
                return elements[i].Attributes["xlink:href"].Value;

            }
            set
            {
                XmlElement[] elements = fields.GetFieldElements(definition).ToArray<XmlElement>();
                if (i >= elements.Length) throw new IndexOutOfRangeException();

                elements[i].SetAttribute("href", "http://www.w3.org/1999/xlink", value);

            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return fields.GetFieldElements(definition).Select<XmlElement, string>(elm => IsLinkField ? elm.Attributes["xlink:href"].Value : elm.InnerXml.ToString()
            ).GetEnumerator();
        }
    }


}