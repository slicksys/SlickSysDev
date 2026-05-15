using System.Net;
using System.Net.Http.Json;
using ManagementData.Api.Contracts.Auth;
using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.Scheduling;

namespace ManagementData.Api.IntegrationTests;

public sealed class AuthAndSchedulingTests : IClassFixture<TestApiFactory>
{
    private readonly TestApiFactory _factory;

    public AuthAndSchedulingTests(TestApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateToken_ReturnsBearerToken()
    {
        await _factory.SeedAsync();
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/auth/token", new TokenRequest
        {
            UsernameOrEmail = TestApiFactory.PracticeUserName,
            Password = TestApiFactory.PracticeUserPassword,
            PracticeId = TestApiFactory.PracticeId,
            RequiredRoleName = "practice_user"
        });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<TokenResponse>();
        Assert.NotNull(payload);
        Assert.False(string.IsNullOrWhiteSpace(payload!.AccessToken));
        Assert.Equal("Bearer", payload.TokenType);
    }

    [Fact]
    public async Task CreateToken_WithInvalidPassword_ReturnsUnauthorized()
    {
        await _factory.SeedAsync();
        using var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/auth/token", new TokenRequest
        {
            UsernameOrEmail = TestApiFactory.PracticeUserName,
            Password = "wrong-password",
            PracticeId = TestApiFactory.PracticeId,
            RequiredRoleName = "practice_user"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetScheduleBoard_RequiresAuth()
    {
        await _factory.SeedAsync();
        using var client = _factory.CreateClient();

        var url = $"/api/v1/practices/{TestApiFactory.PracticeId}/scheduling/schedule-board"
            + "?fromTime=2026-01-01T00:00:00"
            + "&toTime=2026-12-31T23:59:59"
            + "&offsetRows=0&fetchRows=25&sortDesc=false";

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetScheduleBoard_WithToken_ReturnsPagedData()
    {
        await _factory.SeedAsync();
        using var client = await _factory.CreatePracticeUserClientAsync();

        var url = $"/api/v1/practices/{TestApiFactory.PracticeId}/scheduling/schedule-board"
            + "?fromTime=2026-01-01T00:00:00"
            + "&toTime=2026-12-31T23:59:59"
            + "&offsetRows=0&fetchRows=25&sortDesc=false";

        var response = await client.GetAsync(url);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var payload = await response.Content.ReadFromJsonAsync<PagedResponse<ScheduleBoardItemResponse>>();
        Assert.NotNull(payload);
        Assert.True(payload!.TotalRows >= 1);
        Assert.NotEmpty(payload.Items);
    }

    [Fact]
    public async Task GetPracticeInvitations_WithPracticeUserToken_ReturnsForbidden()
    {
        await _factory.SeedAsync();
        using var client = await _factory.CreatePracticeUserClientAsync();

        var response = await client.GetAsync($"/api/v1/practices/{TestApiFactory.PracticeId}/invitations?offsetRows=0&fetchRows=25");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task RevokePracticeInvitation_WithMissingToken_ReturnsNotFound()
    {
        await _factory.SeedAsync();
        using var client = await _factory.CreatePracticeAdminClientAsync();

        var response = await client.PostAsync(
            $"/api/v1/practices/{TestApiFactory.PracticeId}/invitations/{Guid.NewGuid()}/revoke",
            content: null);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task RemoveUserPracticeRole_WithMissingRole_ReturnsNotFound()
    {
        await _factory.SeedAsync();
        using var client = await _factory.CreatePracticeAdminClientAsync();

        var response = await client.DeleteAsync(
            $"/api/v1/practices/{TestApiFactory.PracticeId}/users/{TestApiFactory.PracticeUserId}/roles/practice_admin");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}