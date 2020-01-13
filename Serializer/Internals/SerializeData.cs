using Newtonsoft.Json;
using Serializer.Enteties;
using Serializer.Interfaces;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Serializer.Internals
{
    internal class SerializeData : ISerializeData
    {
        public void SerializeJson(String fileName, Data data)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentOutOfRangeException(nameof(fileName), "File path may not be null, empty or whitespace");
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            String result = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(result);
            }
        }

        public void SerializeXml(String fileName, Data data)
        {
            if (String.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentOutOfRangeException(nameof(fileName), "File path may not be null, empty or whitespace");
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());

            using (StreamWriter streamWriter = new StreamWriter(fileName))
            using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter))
            {
                xmlSerializer.Serialize(xmlWriter, data);
            }
        }
    }
}