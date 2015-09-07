using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace TranslationTracker
{
    public class Database
    {
        private Dictionary<string, string> dictionary;
        private HashSet<string> koreanSet;
        private HashSet<string> englishSet;

        private XmlSerializer serializer;

        public Database()
        {
            serializer = new XmlSerializer(typeof(Item[]), new XmlRootAttribute() { ElementName = "items" });
            dictionary = new Dictionary<string, string>();
            koreanSet = new HashSet<string>();
            englishSet = new HashSet<string>();
        }

        public void InsertItem(string korean, string english)
        {
            dictionary.Add(korean, english);
            koreanSet.Add(korean);
            englishSet.Add(english);
        }

        public List<Tuple<string,string>> SearchString(string searchString)
        {
            List<Tuple<string,string>> list = new List<Tuple<string,string>>();
            


            return list;
        }

        public void LoadDatabase(string path)
        {
            dictionary.Clear();

            if (!File.Exists(path))
            {
                throw new Exception("File does not exist");
            }

            using (FileStream fs = File.OpenRead(path))
            {
                dictionary = ((Item[])serializer.Deserialize(fs))
               .ToDictionary(i => i.Korean, i => i.English);
            }
        }

        public void SaveDatabase(string path)
        {
            using (FileStream fs = File.Create(path))
            {
                serializer.Serialize(fs,
                  dictionary.Select(kv => new Item() { Korean = kv.Key, English = kv.Value }).ToArray());
            }
        }

        public Dictionary<string, string> GetDictionary()
        {
            return dictionary;
        }

    }
}
