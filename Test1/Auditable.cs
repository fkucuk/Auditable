namespace Test1;

public abstract class Auditable : IAuditable
{
    public virtual DateTime CreatedDateUtc { get; set; } = DateTime.UtcNow;
    public virtual DateTime? UpdatedDateUtc { get; set; }
    public virtual DateTime? DeletedDateUtc { get; set; }

    public virtual string CreatedUser { get; set; } = string.Empty;
    public virtual string? UpdatedUser { get; set; }
    public virtual string? DeletedUser { get; set; }

    public virtual bool IsDeleted { get; set; } = false;

}