using Mechanicshop.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Mechanicshop.Infrastructure.RealTime;

public sealed class SignalRWorkOrderNotifier(IHubContext<WorkOrderHub> hubContext) : IWorkOrderNotifier
{
    public Task NotifyWorkOrdersChangedAsync(CancellationToken ct = default) => 
        hubContext.Clients.All.SendAsync("WorkOrdersChanged", cancellationToken: ct);
}