using Serializer.Enteties;
using System;

namespace Serializer.Interfaces
{
    public  interface IDeSerializeData
    {
        Data DeserializeJson(String filePath);

        Data DeserializeXml(String filePath);
    }
}