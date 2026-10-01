using Mechanicshop.Domain.Customers;
using Mechanicshop.Domain.Customers.Vehicles;
using Mechanicshop.Domain.Employees;
using Mechanicshop.Domain.Identity;
using Mechanicshop.Domain.ReapairTasks;
using Mechanicshop.Domain.ReapairTasks.Parts;
using Mechanicshop.Domain.WorkOrder;
using Mechanicshop.Domain.WorkOrder.Billing;
using Microsoft.EntityFrameworkCore;

namespace Mechanicshop.Application.Common.Interfaces;

public interface IAppDbContext
{
    public DbSet<WorkOrder> WorkOrders { get; }
    public DbSet<Customer> Customers { get; }
    public DbSet<RepairTask> RepairTasks { get; }
    public DbSet<Employee> Employees { get; }
    public DbSet<Vehicle> Vehicles { get; }
    public DbSet<Part> Parts { get; }
    public DbSet<Invoice> Invoices { get; }
    public DbSet<RefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}