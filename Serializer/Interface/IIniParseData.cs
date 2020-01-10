using Serializer.Enteties;
using System;

namespace Serializer.Interface
{
    public interface IIniParseData
    {
        void IniWriter(String fileName, Data data);
        Data ReadIni(String fileName);

    }
}
