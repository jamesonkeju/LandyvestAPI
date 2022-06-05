using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Landyvest.Services.DataAccess
{
    public interface IDataAccess
    {
        IDbConnection CreateConnection();

        void CloseConnection(IDbConnection connection);

        IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection);

        IDataAdapter CreateAdapter(IDbCommand command);

        IDbDataParameter CreateParameter(IDbCommand command);
    }
}
