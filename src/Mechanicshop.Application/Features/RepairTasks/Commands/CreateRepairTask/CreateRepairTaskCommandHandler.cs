using Mechanicshop.Application.Common.Interfaces;
using Mechanicshop.Application.Features.RepairTasks.DTOs;
using Mechanicshop.Application.Features.RepairTasks.Mappers;
using Mechanicshop.Domain.Common.Results;
using Mechanicshop.Domain.ReapairTasks;
using Mechanicshop.Domain.ReapairTasks.Parts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Mechanicshop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public class CreateRepairTaskCommandHandler(
    ILogger<CreateRepairTaskCommandHandler> logger,
    IAppDbContext context,
    HybridCache cache) : IRequestHandler<CreateRepairTaskCommand, Result<RepairTaskDTO>>
{
    public async Task<Result<RepairTaskDTO>> Handle(CreateRepairTaskCommand command, CancellationToken cancellationToken)
    {
        var nameExists = await context.RepairTasks
           .AnyAsync(p => EF.Functions.Like(p.Name, command.Name), cancellationToken);

        if (nameExists)
        {
            logger.LogWarning("Duplicate part name '{PartName}'.", command.Name);

            return RepairTaskErrors.DuplicateName;
        }

        List<Part> parts = [];

        foreach (var p in command.Parts)
        {
            var partResult = Part.Create(Guid.NewGuid(), p.Name, p.Cost, p.Quantity);

            if (partResult.IsError)
            {
                return partResult.Errors;
            }

            parts.Add(partResult.Value);
        }

        var createRepairTaskResult = RepairTask.Create(
                    id: Guid.NewGuid(),
                    name: command.Name!,
                    laborCost: command.LaborCost,
                    estimatedDurationInMins: command.EstimatedDurationInMins!.Value,
                    parts: parts);

        if (createRepairTaskResult.IsError)
        {
            return createRepairTaskResult.Errors;
        }
        var repairTask = createRepairTaskResult.Value;

        context.RepairTasks.Add(repairTask);

        await context.SaveChangesAsync(cancellationToken);

        await cache.RemoveByTagAsync("repair-task", cancellationToken);

        return repairTask.ToDto();
    }
}