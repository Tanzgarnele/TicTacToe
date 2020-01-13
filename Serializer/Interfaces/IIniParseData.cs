using Serializer.Enteties;
using System;

namespace Serializer.Interfaces
{
    public interface IIniParseData
    {
        void WriteToIni(String fileName, Data data);

        Data ReadIni(String fileName);
    }
}