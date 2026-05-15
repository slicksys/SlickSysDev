using System.Data;
using Dapper;
using ManagementData.Api.Contracts.Common;
using ManagementData.Api.Contracts.Scheduling;
using ManagementData.Api.Data;

namespace ManagementData.Api.Features.Scheduling;

public sealed class SqlSchedulingRepository(IDbConnectionFactory dbConnectionFactory) : ISchedulingRepository
{
    public async Task<PagedResponse<ScheduleBoardItemResponse>> GetScheduleBoardAsync(Guid practiceId, ScheduleBoardQuery query, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        var items = (await connection.QueryAsync<ScheduleBoardItemResponse>(new CommandDefinition(
            "dbo.usp_schedule_board_paged",
            new
            {
                practice_id = practiceId,
                from_time = query.FromTime,
                to_time = query.ToTime,
                client_id = query.ClientId,
                principal_id = query.PrincipalId,
                status_id = query.StatusId,
                item_type = query.ItemType,
                offset_rows = query.OffsetRows,
                fetch_rows = query.FetchRows,
                sort_desc = query.SortDesc
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken))).AsList();

        var totalRows = await connection.QuerySingleAsync<long>(new CommandDefinition(
            "dbo.usp_schedule_board_count",
            new
            {
                practice_id = practiceId,
                from_time = query.FromTime,
                to_time = query.ToTime,
                client_id = query.ClientId,
                principal_id = query.PrincipalId,
                status_id = query.StatusId,
                item_type = query.ItemType
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));

        return new PagedResponse<ScheduleBoardItemResponse>(items, totalRows, query.OffsetRows, query.FetchRows);
    }

    public async Task<AppointmentResponse> CreateAppointmentAsync(Guid practiceId, CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleAsync<AppointmentResponse>(new CommandDefinition(
            "dbo.usp_create_appointment",
            new
            {
                practice_id = practiceId,
                client_id = request.ClientId,
                principal_id = request.PrincipalId,
                resource_id = request.ResourceId,
                status_id = request.StatusId,
                start_time = request.StartTime,
                end_time = request.EndTime,
                comments = request.Comments,
                group_id = request.GroupId,
                recurrence_id = request.RecurrenceId,
                source = request.Source
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<AppointmentResponse?> UpdateAppointmentAsync(Guid practiceId, Guid appointmentId, UpdateAppointmentRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<AppointmentResponse>(new CommandDefinition(
            "dbo.usp_update_appointment",
            new
            {
                practice_id = practiceId,
                appointment_id = appointmentId,
                status_id = request.StatusId,
                resource_id = request.ResourceId,
                start_time = request.StartTime,
                end_time = request.EndTime,
                comments = request.Comments,
                is_deleted = request.IsDeleted
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<ReservationResponse> CreateReservationAsync(Guid practiceId, CreateReservationRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleAsync<ReservationResponse>(new CommandDefinition(
            "dbo.usp_create_reservation",
            new
            {
                practice_id = practiceId,
                client_id = request.ClientId,
                principal_id = request.PrincipalId,
                arrival_date = request.ArrivalDate,
                ending_date = request.EndingDate,
                status_id = request.StatusId,
                visual_status_id = request.VisualStatusId,
                hospitalized_flag = request.HospitalizedFlag,
                comments = request.Comments,
                source = request.Source
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }

    public async Task<ReservationResponse?> UpdateReservationAsync(Guid practiceId, Guid reservationId, UpdateReservationRequest request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.CreateOpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<ReservationResponse>(new CommandDefinition(
            "dbo.usp_update_reservation",
            new
            {
                practice_id = practiceId,
                reservation_id = reservationId,
                status_id = request.StatusId,
                visual_status_id = request.VisualStatusId,
                arrival_date = request.ArrivalDate,
                ending_date = request.EndingDate,
                hospitalized_flag = request.HospitalizedFlag,
                comments = request.Comments,
                is_deleted = request.IsDeleted
            },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken));
    }
}