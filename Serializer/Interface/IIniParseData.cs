using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public interface IIniParseData
    {
        Int32 Index { get; set; }
        void WriteIni(String fileName, Data data);
        Data ReadIni(String fileName);

    }
}
