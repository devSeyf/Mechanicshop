namespace Mechanicshop.Domain.Common;

public abstract class AuditableEntity : Entity
{
    protected AuditableEntity()
    { }

    protected AuditableEntity(Guid id) : base(id)
    {
    }

    public DateTimeOffset CreatedAtUTC { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedUTC { get; set; }
    public string? LastModifiedBy { get; set; }
}