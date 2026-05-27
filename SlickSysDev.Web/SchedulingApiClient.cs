using System.Net.Http.Json;

namespace SlickSysDev.Web;

public class SchedulingApiClient(HttpClient httpClient)
{
    public async Task<List<TimeSlotDto>> GetAvailableSlotsAsync(DateTime date, CancellationToken ct = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<TimeSlotDto>>(
            $"/appointments/slots?date={date:yyyy-MM-dd}", ct);
        return result ?? [];
    }

    public async Task<AppointmentResult> BookAppointmentAsync(AppointmentRequestDto request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync("/appointments", request, ct);
        if (response.IsSuccessStatusCode)
        {
            var appointment = await response.Content.ReadFromJsonAsync<AppointmentDto>(cancellationToken: ct);
            return new AppointmentResult { Success = true, Appointment = appointment };
        }

        var error = await response.Content.ReadFromJsonAsync<ErrorResponse>(cancellationToken: ct);
        return new AppointmentResult { Success = false, ErrorMessage = error?.Message ?? "Booking failed." };
    }
}

// ── DTOs ────────────────────────────────────────────────────────────

public class TimeSlotDto
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public bool IsAvailable { get; set; }
}

public class AppointmentRequestDto
{
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientPhone { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public string? Notes { get; set; }
}

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

public class AppointmentResult
{
    public bool Success { get; set; }
    public AppointmentDto? Appointment { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ErrorResponse
{
    public string? Message { get; set; }
}
