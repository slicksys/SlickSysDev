using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using ManagementData.Api.Contracts.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ManagementData.Api.IntegrationTests;

public sealed class TestApiFactory : WebApplicationFactory<Program>
{
    public const string ConnectionString = "Server=(localdb)\\ProjectModels;Database=managementdata_e2e;Integrated Security=true;Encrypt=false;TrustServerCertificate=true;";
    public static readonly Guid PracticeId = Guid.Parse("11111111-1111-1111-1111-111111111111");
    public const string PracticeUserId = "integration-user-1";
    public const string PracticeUserName = "integration.user";
    public const string PracticeUserPassword = "Passw0rd!Integration";
    public const string PracticeAdminId = "integration-admin-1";
    public const string PracticeAdminUserName = "integration.admin";
    public const string PracticeAdminPassword = "Passw0rd!Admin";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            var overrides = new Dictionary<string, string?>
            {
                ["ConnectionStrings:ManagementData"] = ConnectionString,
                ["Jwt:Issuer"] = "ManagementData.Api.Tests",
                ["Jwt:Audience"] = "ManagementData.Api.Tests.Client",
                ["Jwt:SigningKey"] = "managementdata-tests-signing-key-change-this-before-production",
                ["Jwt:ExpiryMinutes"] = "60"
            };

            config.AddInMemoryCollection(overrides);
        });
    }

    public async Task SeedAsync()
    {
        var passwordHasher = new PasswordHasher<IdentityUser>();
        var practiceUserPasswordHash = passwordHasher.HashPassword(
            new IdentityUser { Id = PracticeUserId, UserName = PracticeUserName, Email = "integration.user@example.com" },
            PracticeUserPassword);
        var practiceAdminPasswordHash = passwordHasher.HashPassword(
            new IdentityUser { Id = PracticeAdminId, UserName = PracticeAdminUserName, Email = "integration.admin@example.com" },
            PracticeAdminPassword);

        await using var connection = new SqlConnection(ConnectionString);
        await connection.OpenAsync();

        var sql = """
            if not exists (select 1 from dbo.practice where practice_id = '11111111-1111-1111-1111-111111111111')
            begin
                insert into dbo.practice (practice_id, practice_name, is_active)
                values ('11111111-1111-1111-1111-111111111111', N'integration practice', 1);
            end

            if not exists (select 1 from dbo.client where client_id = '22222222-2222-2222-2222-222222222222')
            begin
                insert into dbo.client (client_id, practice_id, client_account_number, client_name, billing_status, credit_limit, is_active)
                values ('22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', N'INT-1', N'integration client', N'watch', 100.00, 1);
            end

            if not exists (select 1 from dbo.principal where principal_id = '33333333-3333-3333-3333-333333333333')
            begin
                insert into dbo.principal (principal_id, practice_id, client_id, display_name, context_label, species, breed, sex, active)
                values ('33333333-3333-3333-3333-333333333333', '11111111-1111-1111-1111-111111111111', '22222222-2222-2222-2222-222222222222', N'integration principal', N'pet', N'canine', N'mix', N'M', 1);
            end

            if not exists (select 1 from dbo.resource where resource_id = '44444444-4444-4444-4444-444444444444')
            begin
                insert into dbo.resource (resource_id, practice_id, resource_name, resource_type, is_active)
                values ('44444444-4444-4444-4444-444444444444', '11111111-1111-1111-1111-111111111111', N'integration room', N'room', 1);
            end

            if not exists (select 1 from dbo.appointment_status where status_id = '55555555-5555-5555-5555-555555555555')
            begin
                insert into dbo.appointment_status (status_id, practice_id, status_name, sort_order, is_active, color_code)
                values ('55555555-5555-5555-5555-555555555555', '11111111-1111-1111-1111-111111111111', N'scheduled', 1, 1, N'#00aaff');
            end

            if not exists (select 1 from dbo.AspNetRoles where Id = N'role_practice_user')
            begin
                insert into dbo.AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
                values (N'role_practice_user', N'practice_user', N'PRACTICE_USER', null);
            end

            if not exists (select 1 from dbo.AspNetRoles where Id = N'role_practice_admin')
            begin
                insert into dbo.AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
                values (N'role_practice_admin', N'practice_admin', N'PRACTICE_ADMIN', null);
            end

            if not exists (select 1 from dbo.AspNetUsers where Id = N'integration-user-1')
            begin
                insert into dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                values (N'integration-user-1', N'integration.user', N'INTEGRATION.USER', N'integration.user@example.com', N'INTEGRATION.USER@EXAMPLE.COM', 1, @practice_user_password_hash, 0, 0, 0, 0);
            end

            update dbo.AspNetUsers
            set PasswordHash = @practice_user_password_hash,
                LockoutEnabled = 0,
                LockoutEnd = null,
                AccessFailedCount = 0
            where Id = N'integration-user-1';

            if not exists (select 1 from dbo.AspNetUsers where Id = N'integration-admin-1')
            begin
                insert into dbo.AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
                values (N'integration-admin-1', N'integration.admin', N'INTEGRATION.ADMIN', N'integration.admin@example.com', N'INTEGRATION.ADMIN@EXAMPLE.COM', 1, @practice_admin_password_hash, 0, 0, 0, 0);
            end

            update dbo.AspNetUsers
            set PasswordHash = @practice_admin_password_hash,
                LockoutEnabled = 0,
                LockoutEnd = null,
                AccessFailedCount = 0
            where Id = N'integration-admin-1';

            if not exists (
                select 1
                from dbo.user_practice_role
                where practice_id = '11111111-1111-1111-1111-111111111111'
                  and user_id = N'integration-user-1'
                  and role_id = N'role_practice_user'
            )
            begin
                insert into dbo.user_practice_role (practice_id, user_id, role_id, is_active)
                values ('11111111-1111-1111-1111-111111111111', N'integration-user-1', N'role_practice_user', 1);
            end

            if not exists (
                select 1
                from dbo.user_practice_role
                where practice_id = '11111111-1111-1111-1111-111111111111'
                  and user_id = N'integration-admin-1'
                  and role_id = N'role_practice_admin'
            )
            begin
                insert into dbo.user_practice_role (practice_id, user_id, role_id, is_active)
                values ('11111111-1111-1111-1111-111111111111', N'integration-admin-1', N'role_practice_admin', 1);
            end

            if not exists (
                select 1
                from dbo.appointment
                where practice_id = '11111111-1111-1111-1111-111111111111'
                  and appointment_id = 'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa'
            )
            begin
                insert into dbo.appointment (
                    appointment_id,
                    practice_id,
                    client_id,
                    principal_id,
                    resource_id,
                    status_id,
                    start_time,
                    end_time,
                    comments,
                    source,
                    is_deleted
                )
                values (
                    'aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa',
                    '11111111-1111-1111-1111-111111111111',
                    '22222222-2222-2222-2222-222222222222',
                    '33333333-3333-3333-3333-333333333333',
                    '44444444-4444-4444-4444-444444444444',
                    '55555555-5555-5555-5555-555555555555',
                    '2026-01-01T10:00:00',
                    '2026-01-01T11:00:00',
                    N'integration appointment',
                    N'integration',
                    0
                );
            end
            """;

        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@practice_user_password_hash", practiceUserPasswordHash);
        command.Parameters.AddWithValue("@practice_admin_password_hash", practiceAdminPasswordHash);
        await command.ExecuteNonQueryAsync();
    }

    public async Task<HttpClient> CreatePracticeUserClientAsync()
    {
        return await CreateAuthenticatedClientAsync(
            PracticeUserName,
            PracticeUserPassword,
            PracticeId,
            "practice_user");
    }

    public async Task<HttpClient> CreatePracticeAdminClientAsync()
    {
        return await CreateAuthenticatedClientAsync(
            PracticeAdminUserName,
            PracticeAdminPassword,
            PracticeId,
            "practice_admin");
    }

    public async Task<HttpClient> CreateAuthenticatedClientAsync(
        string usernameOrEmail,
        string password,
        Guid? practiceId = null,
        string? requiredRoleName = null)
    {
        var client = CreateClient();

        var tokenRequest = new TokenRequest
        {
            UsernameOrEmail = usernameOrEmail,
            Password = password,
            PracticeId = practiceId,
            RequiredRoleName = requiredRoleName
        };

        var response = await client.PostAsJsonAsync("/api/v1/auth/token", tokenRequest);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<TokenResponse>(new JsonSerializerOptions(JsonSerializerDefaults.Web));
        if (payload is null || string.IsNullOrWhiteSpace(payload.AccessToken))
        {
            throw new InvalidOperationException("Failed to obtain access token for integration tests.");
        }

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", payload.AccessToken);
        return client;
    }
}