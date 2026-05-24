using System.Data.Common;

namespace ManagementData.Api.Data;

public interface IDbConnectionFactory
{
    ValueTask<DbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken);
}