# csharp data access examples

## dapper read example

```csharp
const string sql = "dbo.usp_schedule_board_paged";

var rows = await connection.QueryAsync<ScheduleBoardRow>(
    sql,
    new
    {
        practice_id = practiceId,
        from_time = fromTime,
        to_time = toTime,
        client_id = (Guid?)null,
        principal_id = (Guid?)null,
        status_id = (Guid?)null,
        item_type = (string?)null,
        offset_rows = 0,
        fetch_rows = 50,
        sort_desc = false
    },
    commandType: CommandType.StoredProcedure);
```

## dapper command example

```csharp
var created = await connection.QuerySingleAsync<AppointmentRow>(
    "dbo.usp_create_appointment",
    new
    {
        practice_id = practiceId,
        client_id = clientId,
        principal_id = principalId,
        resource_id = resourceId,
        status_id = statusId,
        start_time = startTime,
        end_time = endTime,
        comments = comments,
        group_id = (Guid?)null,
        recurrence_id = (Guid?)null,
        source = "api"
    },
    commandType: CommandType.StoredProcedure);
```

## ef core keyless read model example

```csharp
public sealed class BillingSummaryRow
{
    public Guid PracticeId { get; set; }
    public Guid ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public decimal TotalDue { get; set; }
    public string BillingStatus { get; set; } = string.Empty;
}

// in OnModelCreating:
modelBuilder.Entity<BillingSummaryRow>().HasNoKey();

var data = await db.Set<BillingSummaryRow>()
    .FromSqlInterpolated($@"
        exec dbo.usp_billing_summary_paged
            @practice_id={practiceId},
            @client_id={clientId},
            @billing_status={billingStatus},
            @offset_rows={offset},
            @fetch_rows={take},
            @sort_desc={sortDesc}")
    .ToListAsync();
```

## ado.net command example

```csharp
using var cmd = connection.CreateCommand();
cmd.CommandText = "dbo.usp_post_payment";
cmd.CommandType = CommandType.StoredProcedure;

cmd.Parameters.Add(new SqlParameter("@practice_id", SqlDbType.UniqueIdentifier) { Value = practiceId });
cmd.Parameters.Add(new SqlParameter("@client_id", SqlDbType.UniqueIdentifier) { Value = clientId });
cmd.Parameters.Add(new SqlParameter("@invoice_id", SqlDbType.UniqueIdentifier) { Value = (object?)invoiceId ?? DBNull.Value });
cmd.Parameters.Add(new SqlParameter("@payment_amount", SqlDbType.Decimal) { Value = amount, Precision = 18, Scale = 2 });
cmd.Parameters.Add(new SqlParameter("@payment_date", SqlDbType.DateTime2) { Value = paymentDate });
cmd.Parameters.Add(new SqlParameter("@payment_method", SqlDbType.NVarChar, 50) { Value = paymentMethod });
cmd.Parameters.Add(new SqlParameter("@reference_number", SqlDbType.NVarChar, 100) { Value = (object?)reference ?? DBNull.Value });
cmd.Parameters.Add(new SqlParameter("@memo", SqlDbType.NVarChar, 400) { Value = (object?)memo ?? DBNull.Value });
cmd.Parameters.Add(new SqlParameter("@source", SqlDbType.NVarChar, 50) { Value = "api" });

using var reader = await cmd.ExecuteReaderAsync();
```

## dapper authorization check example

```csharp
var auth = await connection.QuerySingleAsync<PracticeAuthorizationRow>(
    "dbo.usp_authorize_user_practice",
    new
    {
        practice_id = practiceId,
        user_id = userId,
        required_role_name = "practice_admin"
    },
    commandType: CommandType.StoredProcedure);

public sealed class PracticeAuthorizationRow
{
    public Guid PracticeId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? RequiredRoleName { get; set; }
    public bool IsAuthorized { get; set; }
}
```

## dapper invitation accept example

```csharp
var accepted = await connection.QuerySingleAsync<UserPracticeInvitationRow>(
    "dbo.usp_accept_user_practice_invitation",
    new
    {
        invite_token = inviteToken,
        accepted_user_id = userId
    },
    commandType: CommandType.StoredProcedure);

public sealed class UserPracticeInvitationRow
{
    public Guid UserPracticeInvitationId { get; set; }
    public Guid PracticeId { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid InviteToken { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public string? InvitedByUserId { get; set; }
    public string? AcceptedUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? RevokedAt { get; set; }
}
```
