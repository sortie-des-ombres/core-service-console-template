using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Tridion.ContentManager.CoreService.Client;

namespace CoreServiceUtils.Tridion
{

    public class ValueCollection
    {
        private Fields fields;
        private ItemFieldDefinitionData definition;

        public ValueCollection(Fields _fields, ItemFieldDefinitionData _definition)
        {
            fields = _fields;
            definition = _definition;
        }

        public int Count
        {
            get { return fields.GetFieldElements(definition).Count(); }
        }

        public bool Contains(string value)
        {
            List<XmlElement> values = fields.GetFieldElements(definition).ToList<XmlElement>();
            bool success = false;

            var list = from v in values
                       where (IsLinkField && v.Attributes["xlink:href"].Value == value) ||
                                (!IsLinkField && v.InnerXml.ToString() == value)
                       select v;

            success = list.Count() > 0;

            return success;
        }

        public bool IsLinkField
        {
            get { return definition is ComponentLinkFieldDefinitionData || definition is ExternalLinkFieldDefinitionData || definition is MultimediaLinkFieldDefinitionData; }
        }

        public bool IsRichTextField
        {
            get { return definition is XhtmlFieldDefinitionData; }
        }

        public bool IsEmbeddedField
        {
            get { return definition is EmbeddedSchemaFieldDefinitionData; }
        }

        public Dictionary<string, string> GetEmbeddedValue(int i)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            XmlElement[] elements = fields.GetFieldElements(definition).ToArray<XmlElement>();
            if (i >= elements.Length) throw new IndexOutOfRangeException();

            foreach (XmlNode node in elements[i].ChildNodes)
            {
                results.Add(node.Name, node.InnerText);
            }
            return results;
        }

        public string this[int i]
        {
            get
            {
                XmlElement[] elements = fields.GetFieldElements(definition).ToArray();
                if (i >= elements.Length) throw new IndexOutOfRangeException();
                if (IsLinkField)
                {
                    return elements[i].Attributes["xlink:href"].Value;
                }
                else
                {
                    return elements[i].InnerXml.ToString(); // used to be InnerText
                }
            }
            set
            {
                XmlElement[] elements = fields.GetFieldElements(definition).ToArray<XmlElement>();
                if (i >= elements.Length) throw new IndexOutOfRangeException();
                if (IsLinkField)
                {
                    elements[i].SetAttribute("href", "http://www.w3.org/1999/xlink", value);
                    elements[i].SetAttribute("type", "http://www.w3.org/1999/xlink", "simple");
                    if (elements[i].Attributes["href"] != null)
                    {
                        elements[i].Attributes["href"].Prefix = "xlink";
                    }
                    if (elements[i].Attributes["type"] != null)
                    {
                        elements[i].Attributes["type"].Prefix = "xlink";
                    }
                    // TODO: should we clear the title for MMCLink and CLink fields? They will automatically be updated when we save the xlink:href.
                }
                else
                {
                    if (IsRichTextField)
                    {
                        elements[i].InnerXml = value;
                    }
                    else
                    {
                        elements[i].InnerText = value;
                    }
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return fields.GetFieldElements(definition).Select<XmlElement, string>(elm => IsLinkField ? elm.Attributes["xlink:href"].Value : elm.InnerXml.ToString()
            ).GetEnumerator();
        }
    }


}