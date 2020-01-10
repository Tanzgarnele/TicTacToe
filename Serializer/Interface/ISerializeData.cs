using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public interface ISerializeData
    {
        void SerializeJson<Data>(String fileName, Data data);

        void SerializeXml(String fileName, Data data);
    }
}
