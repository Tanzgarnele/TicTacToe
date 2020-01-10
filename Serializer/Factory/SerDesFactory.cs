using Serializer.Interface;
using Serializer.Internal;
using System;

namespace Serializer.Factory
{
    public static class SerDesFactory
    {
        public static Object Create(Type type)
        {
            if (type == typeof(ISerializeData))
            {
                return new SerializeData();
            }

            if (type == typeof(IIniParseData))
            {
                return new IniParseData();
            }

            if (type == typeof(IDeSerializeData))
            {
                return new DeSerializer();
            }

            // TODO: für jedes weiter Interface (plus Impl.) ein neues IF analog zu oben!!!

            throw new NotSupportedException();
        }
    }
}