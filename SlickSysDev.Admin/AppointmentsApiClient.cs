using System.Net.Http.Json;

namespace SlickSysDev.Admin;

public class AppointmentsApiClient(HttpClient httpClient)
{
    public async Task<List<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken ct = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<AppointmentDto>>("/appointments", ct);
        return result ?? [];
    }

    public async Task<bool> ConfirmAppointmentAsync(Guid id, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsync($"/appointments/{id}/confirm", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> CancelAppointmentAsync(Guid id, CancellationToken ct = default)
    {
        var response = await httpClient.PutAsync($"/appointments/{id}/cancel", null, ct);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<AdminNotificationDto>> GetNotificationsAsync(CancellationToken ct = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<AdminNotificationDto>>("/notifications", ct);
        return result ?? [];
    }

    public async Task<int> GetUnreadCountAsync(CancellationToken ct = default)
    {
        var result = await httpClient.GetFromJsonAsync<UnreadCountResponse>("/notifications/unread-count", ct);
        return result?.Count ?? 0;
    }

    public async Task MarkAllNotificationsReadAsync(CancellationToken ct = default)
    {
        await httpClient.PutAsync("/notifications/read-all", null, ct);
    }
}

// ── DTOs ────────────────────────────────────────────────────────────

public class AppointmentDto
{
    public Guid Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AdminNotificationDto
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public Guid? AppointmentId { get; set; }
}

public class UnreadCountResponse
{
    public int Count { get; set; }
}
