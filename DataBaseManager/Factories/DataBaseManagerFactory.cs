﻿using DataBaseManager.Interfaces;
using DataBaseManager.Internals;
using System;

namespace DataBaseManager.Factories
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

            throw new NotSupportedException();
        }
    }
}
