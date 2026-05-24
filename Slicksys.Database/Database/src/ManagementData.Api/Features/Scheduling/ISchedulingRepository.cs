using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.Scheduling;

namespace ManagementData.Api.Features.Scheduling;

public interface ISchedulingRepository
{
    Task<PagedResponse<ScheduleBoardItemResponse>> GetScheduleBoardAsync(Guid practiceId, ScheduleBoardQuery query, CancellationToken cancellationToken);
    Task<AppointmentResponse> CreateAppointmentAsync(Guid practiceId, CreateAppointmentRequest request, CancellationToken cancellationToken);
    Task<AppointmentResponse?> UpdateAppointmentAsync(Guid practiceId, Guid appointmentId, UpdateAppointmentRequest request, CancellationToken cancellationToken);
    Task<ReservationResponse> CreateReservationAsync(Guid practiceId, CreateReservationRequest request, CancellationToken cancellationToken);
    Task<ReservationResponse?> UpdateReservationAsync(Guid practiceId, Guid reservationId, UpdateReservationRequest request, CancellationToken cancellationToken);
}