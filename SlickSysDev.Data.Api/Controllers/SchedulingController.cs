using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.Scheduling;
using ManagementData.Api.Features.Auth;
using ManagementData.Api.Features.Scheduling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementData.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/practices/{practiceId:guid}/scheduling")]
public sealed class SchedulingController(ISchedulingRepository repository) : ControllerBase
{
    [HttpGet("schedule-board")]
    [Authorize(Policy = AuthorizationPolicies.PracticeMember)]
    [ProducesResponseType(typeof(PagedResponse<ScheduleBoardItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<PagedResponse<ScheduleBoardItemResponse>>> GetScheduleBoard(
        Guid practiceId,
        [FromQuery] ScheduleBoardQuery query,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetScheduleBoardAsync(practiceId, query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("appointments")]
    [Authorize(Policy = AuthorizationPolicies.PracticeUser)]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AppointmentResponse>> CreateAppointment(
        Guid practiceId,
        [FromBody] CreateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.CreateAppointmentAsync(practiceId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("appointments/{appointmentId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PracticeUser)]
    [ProducesResponseType(typeof(AppointmentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentResponse>> UpdateAppointment(
        Guid practiceId,
        Guid appointmentId,
        [FromBody] UpdateAppointmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.UpdateAppointmentAsync(practiceId, appointmentId, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("reservations")]
    [Authorize(Policy = AuthorizationPolicies.PracticeUser)]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ReservationResponse>> CreateReservation(
        Guid practiceId,
        [FromBody] CreateReservationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.CreateReservationAsync(practiceId, request, cancellationToken);
        return Ok(result);
    }

    [HttpPatch("reservations/{reservationId:guid}")]
    [Authorize(Policy = AuthorizationPolicies.PracticeUser)]
    [ProducesResponseType(typeof(ReservationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReservationResponse>> UpdateReservation(
        Guid practiceId,
        Guid reservationId,
        [FromBody] UpdateReservationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await repository.UpdateReservationAsync(practiceId, reservationId, request, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}