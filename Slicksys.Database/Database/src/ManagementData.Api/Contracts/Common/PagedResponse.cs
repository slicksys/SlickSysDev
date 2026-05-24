namespace ManagementData.Api.Contracts.Common;

public sealed record PagedResponse<T>(
    IReadOnlyCollection<T> Items,
    long TotalRows,
    int OffsetRows,
    int FetchRows);