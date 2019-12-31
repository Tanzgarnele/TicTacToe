using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public  interface IDeSerializeData
    {
        Data JsonDeserialize(String filePath);

        Data XmlDeserialize(String filePath);
    }
}