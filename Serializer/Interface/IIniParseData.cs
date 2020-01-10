using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public interface IIniParseData
    {
        void WriteToIni(String fileName, Data data);
        Data ReadIni(String fileName);

    }
}
