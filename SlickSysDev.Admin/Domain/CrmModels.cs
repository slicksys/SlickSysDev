namespace SlickSysDev.Admin.Domain;

public enum ServiceVertical
{
    Grooming,
    Plumbing
}

public enum DispatchPriority
{
    Standard,
    Emergency
}

public sealed record CustomerLead(
    string Name,
    string Phone,
    ServiceVertical Vertical,
    string IssueSummary);

public sealed record EstimateResult(
    ServiceVertical Vertical,
    string VisionSummary,
    decimal BaseQuote,
    int EstimatedMinutes,
    DispatchPriority Priority,
    IReadOnlyList<string> RecommendedTasks);

public sealed record Appointment(
    Guid Id,
    string Customer,
    ServiceVertical Vertical,
    DateOnly Date,
    TimeOnly Start,
    int DurationMinutes,
    DispatchPriority Priority,
    decimal Quote,
    string Source);

public sealed record AccountingEntry(
    Guid Id,
    DateOnly Date,
    string Category,
    string Description,
    decimal Amount,
    bool IsIncome);

public sealed record DispatchCall(
    Guid Id,
    DateTimeOffset Timestamp,
    string Caller,
    string TranscriptSummary,
    DispatchPriority Priority,
    DateOnly AppointmentDate,
    TimeOnly AppointmentStart,
    int DurationMinutes,
    string RouteCluster);
