using Newtonsoft.Json;
using Serializer.Enteties;
using Serializer.Interfaces;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Serializer.Internals
{
    internal class DeSerializer : IDeSerializeData
    {
        public Data DeserializeJson(String filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentOutOfRangeException(nameof(filePath), "File path may not be null, empty or consist of white-space characters.");
            }

            using (StreamReader readFile = File.OpenText(filePath))
            {
                Data readList = JsonConvert.DeserializeObject<Data>(readFile.ReadToEnd());

                return readList;
            }
        }

        public Data DeserializeXml(String filePath)
        {
            if (String.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentOutOfRangeException(nameof(filePath), "File path may not be null, empty or consist of white-space characters.");
            }

            using (StreamReader readFile = File.OpenText(filePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Data));

                Data readList = (Data)xmlSerializer.Deserialize(readFile);

                return readList;
            }
        }
    }
}