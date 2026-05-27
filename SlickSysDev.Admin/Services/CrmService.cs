using SlickSysDev.Admin.Domain;

namespace SlickSysDev.Admin.Services;

public sealed class CrmService
{
    private readonly List<Appointment> _appointments =
    [
        new(Guid.NewGuid(), "Mia Lopez", ServiceVertical.Grooming, DateOnly.FromDateTime(DateTime.Today), new TimeOnly(9, 0), 90, DispatchPriority.Standard, 145m, "Vision estimator"),
        new(Guid.NewGuid(), "Jordan Bell", ServiceVertical.Plumbing, DateOnly.FromDateTime(DateTime.Today), new TimeOnly(11, 0), 120, DispatchPriority.Emergency, 425m, "Voice dispatcher"),
        new(Guid.NewGuid(), "Riley Chen", ServiceVertical.Grooming, DateOnly.FromDateTime(DateTime.Today.AddDays(1)), new TimeOnly(10, 30), 60, DispatchPriority.Standard, 95m, "Manual")
    ];

    private readonly List<AccountingEntry> _ledger =
    [
        new(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), "Revenue", "Completed grooming", 145m, true),
        new(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today), "Supplies", "Shampoo and blades", 43m, false),
        new(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Revenue", "Emergency plumbing visit", 425m, true),
        new(Guid.NewGuid(), DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), "Fuel", "Service van", 70m, false)
    ];

    private readonly List<DispatchCall> _calls = [];

    public IReadOnlyList<Appointment> Appointments => _appointments.OrderBy(a => a.Date).ThenBy(a => a.Start).ToList();

    public IReadOnlyList<AccountingEntry> Ledger => _ledger.OrderByDescending(l => l.Date).ToList();

    public IReadOnlyList<DispatchCall> Calls => _calls.OrderByDescending(c => c.Timestamp).ToList();

    public decimal TotalRevenue => _ledger.Where(e => e.IsIncome).Sum(e => e.Amount);

    public decimal TotalExpense => _ledger.Where(e => !e.IsIncome).Sum(e => e.Amount);

    public decimal NetIncome => TotalRevenue - TotalExpense;

    public EstimateResult RunVisualEstimate(ServiceVertical vertical, string filename, string issueText)
    {
        var lower = string.Concat(filename, " ", issueText).ToLowerInvariant();

        if (vertical == ServiceVertical.Grooming)
        {
            var matted = lower.Contains("matted") || lower.Contains("double coat") || lower.Contains("doodle");
            var quote = matted ? 180m : 110m;
            var minutes = matted ? 120 : 75;

            return new EstimateResult(
                vertical,
                matted
                    ? "Vision AI detected likely severe matting and dense coat."
                    : "Vision AI detected moderate coat maintenance needs.",
                quote,
                minutes,
                DispatchPriority.Standard,
                matted
                    ? ["De-matting package", "Nail trim", "Sanitary trim"]
                    : ["Bath and brush", "Nail trim"]);
        }

        var emergency = lower.Contains("burst") || lower.Contains("flood") || lower.Contains("active leak");
        var pvc = lower.Contains("pvc");
        var material = pvc ? "PVC" : "Copper";
        var baseQuote = emergency ? 475m : 230m;
        var duration = emergency ? 150 : 90;

        return new EstimateResult(
            vertical,
            $"Vision AI identified {material} piping with {(emergency ? "high" : "moderate")} visible damage.",
            baseQuote,
            duration,
            emergency ? DispatchPriority.Emergency : DispatchPriority.Standard,
            emergency
                ? ["Emergency dispatch", "Water shutoff verification", "Pipe section replacement"]
                : ["Leak inspection", "Fitting replacement"]);
    }

    public Appointment BookFromEstimate(string customer, EstimateResult estimate, DateOnly date, TimeOnly start, string source)
    {
        var appointment = new Appointment(
            Guid.NewGuid(),
            customer,
            estimate.Vertical,
            date,
            start,
            estimate.EstimatedMinutes,
            estimate.Priority,
            estimate.BaseQuote,
            source);

        _appointments.Add(appointment);

        return appointment;
    }

    public DispatchCall CreateVoiceDispatch(string caller, ServiceVertical vertical, string transcriptSummary)
    {
        var estimate = RunVisualEstimate(vertical, "call-intake", transcriptSummary);
        var nextSlot = DateTime.Now.AddHours(2);

        var appointment = BookFromEstimate(
            caller,
            estimate,
            DateOnly.FromDateTime(nextSlot.Date),
            TimeOnly.FromDateTime(nextSlot),
            "Voice dispatcher");

        var call = new DispatchCall(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            caller,
            transcriptSummary,
            estimate.Priority,
            appointment.Date,
            appointment.Start,
            appointment.DurationMinutes,
            estimate.Priority == DispatchPriority.Emergency ? "Immediate response zone" : "Standard route cluster B");

        _calls.Add(call);

        return call;
    }
}
