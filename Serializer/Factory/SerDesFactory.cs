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
                return new SerializeData(); // TODO: Implementierung machen und hier erzeugen und zurückgeben.
            }

            if (type == typeof(IIniParseData))
            {
                return new IniParseData(); // TODO: Implementierung machen und hier erzeugen und zurückgeben.
            }

            if (type == typeof(IDeSerializeData))
            {
                return new DeSerializer(); // TODO: Implementierung machen und hier erzeugen und zurückgeben.
            }

            // TODO: für jedes weiter Interface (plus Impl.) ein neues IF analog zu oben!!!

            throw new NotSupportedException();
        }
    }
}