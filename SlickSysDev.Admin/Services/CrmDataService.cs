using System.Globalization;

namespace SlickSysDev.Admin.Services;

public sealed class CrmDataService
{
    private readonly List<AppointmentRecord> _appointments =
    [
        new("A-1001", "Paws & Shine", ServiceIndustry.Grooming, "Labradoodle Full Groom", DateTime.Today.AddHours(9), 120, 145m, AppointmentPriority.Standard, "Booked from visual estimator"),
        new("A-1002", "Oak Street Family", ServiceIndustry.Plumbing, "Emergency leak triage", DateTime.Today.AddHours(11), 90, 265m, AppointmentPriority.Emergency, "Voice dispatcher scheduled"
        )
    ];

    private readonly List<InvoiceRecord> _invoices =
    [
        new("INV-2201", "Paws & Shine", 145m, PaymentStatus.Paid, DateTime.Today.AddDays(-2)),
        new("INV-2202", "Oak Street Family", 265m, PaymentStatus.Pending, DateTime.Today.AddDays(-1)),
        new("INV-2203", "Mila's Mobile Grooming", 89m, PaymentStatus.Overdue, DateTime.Today.AddDays(-10))
    ];

    private readonly List<CallLog> _calls =
    [
        new(DateTime.Now.AddMinutes(-55), "(555) 018-1212", "Pipe burst in basement", "Accepted by AI", AppointmentPriority.Emergency),
        new(DateTime.Now.AddMinutes(-18), "(555) 018-7373", "Golden Retriever matted coat", "Quoted + booked", AppointmentPriority.Standard)
    ];

    public IReadOnlyList<AppointmentRecord> GetAppointments() => _appointments.OrderBy(a => a.Start).ToList();

    public IReadOnlyList<InvoiceRecord> GetInvoices() => _invoices.OrderByDescending(i => i.IssueDate).ToList();

    public IReadOnlyList<CallLog> GetCallLogs() => _calls.OrderByDescending(c => c.Timestamp).ToList();

    public CrmKpi GetKpis()
    {
        var today = DateTime.Today;
        var todaysJobs = _appointments.Count(a => a.Start.Date == today);
        var emergencyRate = _appointments.Count == 0 ? 0 : _appointments.Count(a => a.Priority == AppointmentPriority.Emergency) / (decimal)_appointments.Count;
        var outstanding = _invoices.Where(i => i.Status != PaymentStatus.Paid).Sum(i => i.Amount);
        var monthlyRevenue = _invoices.Where(i => i.IssueDate.Month == today.Month && i.IssueDate.Year == today.Year).Sum(i => i.Amount);

        return new CrmKpi(todaysJobs, emergencyRate, outstanding, monthlyRevenue);
    }

    public VisualEstimateResult GenerateVisualEstimate(EstimatorInput input)
    {
        var descriptor = $"{input.ServiceType} | {input.PhotoName}";
        if (input.Industry == ServiceIndustry.Grooming)
        {
            var sizeMultiplier = input.Size switch
            {
                PetSize.Small => 1m,
                PetSize.Medium => 1.35m,
                PetSize.Large => 1.8m,
                _ => 1m
            };

            var coatMultiplier = input.CoatCondition switch
            {
                CoatCondition.Healthy => 1m,
                CoatCondition.DoubleCoatOvergrown => 1.45m,
                CoatCondition.SevereMatting => 1.9m,
                _ => 1m
            };

            var amount = decimal.Round(65m * sizeMultiplier * coatMultiplier, 2);
            var duration = (int)Math.Round(50 * (double)(sizeMultiplier * coatMultiplier));
            return new VisualEstimateResult(
                descriptor,
                amount,
                duration,
                AppointmentPriority.Standard,
                $"Vision AI predicts {input.Size} pet with {ToFriendly(input.CoatCondition)}."
            );
        }

        var materialMultiplier = input.PipeMaterial switch
        {
            PipeMaterial.Pvc => 1m,
            PipeMaterial.Copper => 1.45m,
            PipeMaterial.GalvanizedSteel => 1.75m,
            _ => 1m
        };
        var damageMultiplier = input.VisibleDamage switch
        {
            DamageSeverity.MinorLeak => 1m,
            DamageSeverity.ActiveLeak => 1.6m,
            DamageSeverity.BurstOrFlooding => 2.4m,
            _ => 1m
        };

        var cost = decimal.Round(110m * materialMultiplier * damageMultiplier, 2);
        var minutes = (int)Math.Round(45 * (double)(materialMultiplier * damageMultiplier));
        var priority = input.VisibleDamage == DamageSeverity.BurstOrFlooding ? AppointmentPriority.Emergency : AppointmentPriority.Standard;

        return new VisualEstimateResult(
            descriptor,
            cost,
            minutes,
            priority,
            $"Vision AI detected {ToFriendly(input.PipeMaterial)} with {ToFriendly(input.VisibleDamage)}."
        );
    }

    public AppointmentRecord BookAppointment(string customerName, ServiceIndustry industry, string serviceType, DateTime start, VisualEstimateResult estimate)
    {
        var id = $"A-{1000 + _appointments.Count + 1}";
        var appointment = new AppointmentRecord(id, customerName, industry, serviceType, start, estimate.DurationMinutes, estimate.EstimatedPrice, estimate.Priority, estimate.Summary);
        _appointments.Add(appointment);
        _calls.Add(new CallLog(DateTime.Now, "(auto)", serviceType, "Booked", estimate.Priority));

        _invoices.Add(new InvoiceRecord($"INV-{2200 + _invoices.Count + 1}", customerName, estimate.EstimatedPrice, PaymentStatus.Pending, DateTime.Today));
        return appointment;
    }

    public VoiceDispatchResult RunDispatch(DispatchRequest request)
    {
        var start = DateTime.Today.AddHours(request.IsEmergency ? 8 : 13).AddMinutes((_appointments.Count * 25) % 180);
        var priority = request.IsEmergency ? AppointmentPriority.Emergency : AppointmentPriority.Standard;
        var recommendation = request.IsEmergency
            ? "Nearest technician rerouted. Customer receives emergency SLA confirmation."
            : "Customer offered first available route-consistent slot.";

        var appointment = BookAppointment(request.CallerName, request.Industry, request.ServiceDescription, start, new VisualEstimateResult(
            "Voice dispatch baseline",
            request.IsEmergency ? 275m : 140m,
            request.IsEmergency ? 90 : 60,
            priority,
            recommendation
        ));

        _calls.Add(new CallLog(DateTime.Now, request.Phone, request.ServiceDescription, "AI dispatcher completed", priority));

        return new VoiceDispatchResult(appointment, recommendation);
    }

    private static string ToFriendly<T>(T value) where T : struct, Enum
    {
        var text = value.ToString();
        return string.Concat(text.Select((x, i) => i > 0 && char.IsUpper(x) ? $" {x}" : x.ToString()));
    }
}

public record CrmKpi(int JobsToday, decimal EmergencyRate, decimal OutstandingReceivables, decimal MonthlyRevenue)
{
    public string EmergencyRateDisplay => EmergencyRate.ToString("P0", CultureInfo.InvariantCulture);
}

public record AppointmentRecord(
    string Id,
    string Customer,
    ServiceIndustry Industry,
    string ServiceType,
    DateTime Start,
    int DurationMinutes,
    decimal EstimatedPrice,
    AppointmentPriority Priority,
    string Notes
);

public record InvoiceRecord(string Id, string Customer, decimal Amount, PaymentStatus Status, DateTime IssueDate);

public record CallLog(DateTime Timestamp, string Phone, string Topic, string Outcome, AppointmentPriority Priority);

public class EstimatorInput
{
    public ServiceIndustry Industry { get; set; } = ServiceIndustry.Grooming;
    public string PhotoName { get; set; } = "no-file";
    public string ServiceType { get; set; } = "New service";
    public PetSize Size { get; set; } = PetSize.Medium;
    public CoatCondition CoatCondition { get; set; } = CoatCondition.Healthy;
    public PipeMaterial PipeMaterial { get; set; } = PipeMaterial.Pvc;
    public DamageSeverity VisibleDamage { get; set; } = DamageSeverity.MinorLeak;
}

public record VisualEstimateResult(string Descriptor, decimal EstimatedPrice, int DurationMinutes, AppointmentPriority Priority, string Summary);

public class DispatchRequest
{
    public string CallerName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public ServiceIndustry Industry { get; set; } = ServiceIndustry.Plumbing;
    public string ServiceDescription { get; set; } = string.Empty;
    public bool IsEmergency { get; set; }
}

public record VoiceDispatchResult(AppointmentRecord Appointment, string Recommendation);

public enum ServiceIndustry
{
    Grooming,
    Plumbing
}

public enum AppointmentPriority
{
    Standard,
    Emergency
}

public enum PaymentStatus
{
    Paid,
    Pending,
    Overdue
}

public enum PetSize
{
    Small,
    Medium,
    Large
}

public enum CoatCondition
{
    Healthy,
    DoubleCoatOvergrown,
    SevereMatting
}

public enum PipeMaterial
{
    Pvc,
    Copper,
    GalvanizedSteel
}

public enum DamageSeverity
{
    MinorLeak,
    ActiveLeak,
    BurstOrFlooding
}
