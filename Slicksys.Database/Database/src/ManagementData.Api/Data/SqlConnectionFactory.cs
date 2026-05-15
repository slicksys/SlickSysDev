using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace ManagementData.Api.Data;

public sealed class SqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string _connectionString = configuration.GetConnectionString("ManagementData")
        ?? throw new InvalidOperationException("ConnectionStrings:ManagementData is not configured.");

    public async ValueTask<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken)
    {
        var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        return connection;
    }
}