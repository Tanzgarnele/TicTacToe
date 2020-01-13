using DataBaseManager.Interface;
using DataBaseManager.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseManager.Factory
{
    public static class DataBaseManagerFactory
    {
        public static Object Create(Type type)
        {
            if (type == typeof(IDataBaseConnection))
            {
                return new DataBaseConnection();
            }

            if (type == typeof(IDataBaseWriter))
            {
                return new DataBaseWriter();
            }

            // TODO: für jedes weiter Interface (plus Impl.) ein neues IF analog zu oben!!!

            throw new NotSupportedException();
        }
    }
}
