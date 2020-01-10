using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public  interface IDeSerializeData
    {
        Data DeserializeJson(String filePath);

        Data DeserializeXml(String filePath);
    }
}