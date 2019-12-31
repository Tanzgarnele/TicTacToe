using Newtonsoft.Json;
using Serializer.Enteties;
using Serializer.Interface;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Serializer.Internal
{
    internal class DeSerializer : IDeSerializeData
    {
        public Data JsonDeserialize(String filePath)
        {
            using (StreamReader readFile = File.OpenText(filePath))
            {
                Data readList = JsonConvert.DeserializeObject<Data>(readFile.ReadToEnd());

                return readList;
            }
        }

        public Data XmlDeserialize(String filePath)
        {
            using (StreamReader readFile = File.OpenText(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));

                Data readList = (Data)xmlSerializer.Deserialize(readFile);

                return readList;
            }
        }
    }
}