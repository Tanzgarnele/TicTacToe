using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public interface ISerializeData
    {
        void JsonSerialize<Data>(String fileName, Data data);

        void XmlSerialize(String fileName, Data data);
    }
}
