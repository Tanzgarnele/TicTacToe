using Serializer.Enteties;
using System;

namespace Serializer.Interfaces
{
    public interface ISerializeData
    {
        void SerializeJson(String fileName, Data data);

        void SerializeXml(String fileName, Data data);
    }
}