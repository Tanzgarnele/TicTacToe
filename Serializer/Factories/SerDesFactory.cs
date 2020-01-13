using Serializer.Interfaces;
using Serializer.Internals;
using System;

namespace Serializer.Factories
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

            throw new NotSupportedException();
        }
    }
}