using Newtonsoft.Json;
using Serializer.Enteties;
using Serializer.Interface;
using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Serializer.Internal
{
    internal class SerializeData : ISerializeData
    {
        public void JsonSerialize<Data>(String fileName, Data data)
        {
            String result = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.Write(result);
            }
        }

        public void XmlSerialize(String fileName, Data data)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());

            using (StreamWriter streamWriter = new StreamWriter(fileName))
            using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter))
            {
                xmlSerializer.Serialize(xmlWriter, data);
            }
        }
    }
}