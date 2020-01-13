using Serializer.Enteties;
using System;
using System.Threading.Tasks;

namespace Serializer.Interfaces
{
    public interface IDeSerializeData
    {
        Data DeserializeJson(String filePath);

        Data DeserializeXml(String filePath);
    }
}