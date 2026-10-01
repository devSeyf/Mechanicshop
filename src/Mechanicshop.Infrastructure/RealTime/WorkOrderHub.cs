using Microsoft.AspNetCore.SignalR;

namespace Mechanicshop.Infrastructure.RealTime;

public sealed class WorkOrderHub : Hub
{
    public const string HubUrl = "/hubs/workorders";
}