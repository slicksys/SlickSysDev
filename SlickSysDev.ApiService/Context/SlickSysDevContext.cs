using System.Data;
using Microsoft.Data.SqlClient;

namespace SlickSysDev.Data.Service.Context
{
    public interface IDbConnectionProvider
    {
        IDbConnection Connection { get; }
    }

    public class SlickSysDevContext : IDbConnectionProvider
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;
        public SlickSysDevContext(string connection)
        {
            using var _ = _connection = new SqlConnection(connection);
        }

        public IDbConnection Connection { get => _connection; }
        public IDbTransaction Transaction { get => _transaction; set => _transaction = value; }

    }
}


