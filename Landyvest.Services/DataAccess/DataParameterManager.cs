using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.DataAccess
{
    public class DataParameterManager
    {
        public static IDbDataParameter CreateParameter(string providerName, string name, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            IDbDataParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    return CreateSqlParameter(name, value, dbType, direction);

                case "microsoft.entityframeworkcore.sqlserver":
                    return CreateSqlParameter(name, value, dbType, direction);

               
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }

            return parameter;
        }

        public static IDbDataParameter CreateParameter(string providerName, string name, int size, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            IDbDataParameter parameter = null;
            switch (providerName.ToLower())
            {
                case "system.data.sqlclient":
                    return CreateSqlParameter(name, size, value, dbType, direction);
               
                case "system.data.oleDb":
                    break;
                case "system.data.odbc":
                    break;
            }

            return parameter;
        }

        private static IDbDataParameter CreateSqlParameter(string name, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }

        private static IDbDataParameter CreateSqlParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                DbType = dbType,
                Size = size,
                ParameterName = name,
                Direction = direction,
                Value = value
            };
        }


    }
}
