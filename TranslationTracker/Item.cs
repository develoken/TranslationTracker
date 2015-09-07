using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TranslationTracker
{
    public class Item
    {
        [XmlAttribute]
        public string Korean { get; set; }

        [XmlAttribute]
        public string English { get; set; }

        public Item()
        {
        }

        public Item(string Korean, string English)
        {
            this.Korean = Korean;
            this.English = English;
        }

    }
}
