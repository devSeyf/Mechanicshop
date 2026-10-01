using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.WorkOrder.Enums;

namespace Mechanicshop.Application.Common.Interfaces;

public interface IWorkOrderPolicy
{
    bool IsOutsideOperatingHours(DateTimeOffset startAt, TimeSpan duration);

    Task<bool> IsLaborOccupied(Guid laborId, Guid excludedWorkOrderId, DateTimeOffset startAt, DateTimeOffset endAt);

    Task<bool> IsVehicleAlreadyScheduled(Guid vehicleId, DateTimeOffset startAt, DateTimeOffset endAt, Guid? excludedWorkOrderId = null);

    Task<Result<Success>> CheckSpotAvailabilityAsync(Spot spot, DateTimeOffset startAt, DateTimeOffset endAt, Guid? excludeWorkOrderId = null, CancellationToken ct = default);

    Result<Success> ValidateMinimumRequirement(DateTimeOffset startAt, DateTimeOffset endAt);
}