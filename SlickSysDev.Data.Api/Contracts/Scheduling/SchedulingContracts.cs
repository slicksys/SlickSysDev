using System.ComponentModel.DataAnnotations;
using ManagementData.Api.Contracts.Common;

namespace ManagementData.Api.Contracts.Scheduling;

public sealed class ScheduleBoardQuery
{
    [Required]
    public DateTime FromTime { get; init; }

    [Required]
    public DateTime ToTime { get; init; }

    public Guid? ClientId { get; init; }
    public Guid? PrincipalId { get; init; }
    public Guid? StatusId { get; init; }

    [MaxLength(20)]
    public string? ItemType { get; init; }

    [Range(0, int.MaxValue)]
    public int OffsetRows { get; init; }

    [Range(1, 500)]
    public int FetchRows { get; init; } = 50;

    public bool SortDesc { get; init; }
}

public sealed class CreateAppointmentRequest
{
    [Required]
    public Guid ClientId { get; init; }

    public Guid? PrincipalId { get; init; }
    public Guid? ResourceId { get; init; }

    [Required]
    public Guid StatusId { get; init; }

    [Required]
    public DateTime StartTime { get; init; }

    [Required]
    public DateTime EndTime { get; init; }

    [MaxLength(2000)]
    public string? Comments { get; init; }

    public Guid? GroupId { get; init; }
    public Guid? RecurrenceId { get; init; }

    [MaxLength(50)]
    public string? Source { get; init; }
}

public sealed class UpdateAppointmentRequest
{
    public Guid? StatusId { get; init; }
    public Guid? ResourceId { get; init; }
    public DateTime? StartTime { get; init; }
    public DateTime? EndTime { get; init; }

    [MaxLength(2000)]
    public string? Comments { get; init; }

    public bool? IsDeleted { get; init; }
}

public sealed class CreateReservationRequest
{
    [Required]
    public Guid ClientId { get; init; }

    public Guid? PrincipalId { get; init; }

    [Required]
    public DateTime ArrivalDate { get; init; }

    [Required]
    public DateTime EndingDate { get; init; }

    [Required]
    public Guid StatusId { get; init; }

    public Guid? VisualStatusId { get; init; }
    public bool HospitalizedFlag { get; init; }

    [MaxLength(2000)]
    public string? Comments { get; init; }

    [MaxLength(50)]
    public string? Source { get; init; }
}

public sealed class UpdateReservationRequest
{
    public Guid? StatusId { get; init; }
    public Guid? VisualStatusId { get; init; }
    public DateTime? ArrivalDate { get; init; }
    public DateTime? EndingDate { get; init; }
    public bool? HospitalizedFlag { get; init; }

    [MaxLength(2000)]
    public string? Comments { get; init; }

    public bool? IsDeleted { get; init; }
}

public sealed class ScheduleBoardItemResponse
{
    public Guid PracticeId { get; init; }
    public string ItemType { get; init; } = string.Empty;
    public Guid? AppointmentId { get; init; }
    public Guid? ReservationId { get; init; }
    public Guid ClientId { get; init; }
    public Guid? PrincipalId { get; init; }
    public Guid? ResourceId { get; init; }
    public string? ResourceName { get; init; }
    public string? ResourceType { get; init; }
    public Guid StatusId { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string? Comments { get; init; }
    public Guid? GroupId { get; init; }
    public Guid? RecurrenceId { get; init; }
    public bool IsDeleted { get; init; }
}

public sealed class AppointmentResponse
{
    public Guid AppointmentId { get; init; }
    public Guid PracticeId { get; init; }
    public Guid ClientId { get; init; }
    public Guid? PrincipalId { get; init; }
    public Guid? ResourceId { get; init; }
    public Guid StatusId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string? Comments { get; init; }
    public Guid? GroupId { get; init; }
    public Guid? RecurrenceId { get; init; }
    public string? Source { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedAt { get; init; }
}

public sealed class ReservationResponse
{
    public Guid ReservationId { get; init; }
    public Guid PracticeId { get; init; }
    public Guid ClientId { get; init; }
    public Guid? PrincipalId { get; init; }
    public DateTime ArrivalDate { get; init; }
    public DateTime EndingDate { get; init; }
    public Guid StatusId { get; init; }
    public Guid? VisualStatusId { get; init; }
    public bool HospitalizedFlag { get; init; }
    public string? Comments { get; init; }
    public string? Source { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedAt { get; init; }
}