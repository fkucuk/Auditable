using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Test1;


public class AuditableEntitiesInterceptor : SaveChangesInterceptor
{
    private readonly IAuditableHelper _auditableHelper;

    public AuditableEntitiesInterceptor(IAuditableHelper auditableHelper)
    {
        _auditableHelper = auditableHelper;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    _auditableHelper.SetCreatedProperties(entry.Entity);
                    break;
                case EntityState.Deleted:
                    _auditableHelper.SetDeletedProperties(entry.Entity);
                    break;
                case EntityState.Modified:
                    _auditableHelper.SetUpdatedProperties(entry.Entity);
                    break;
                default:
                    break;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(eventData, result);
        }

        var entries = eventData.Context.ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    _auditableHelper.SetCreatedProperties(entry.Entity);
                    break;
                case EntityState.Deleted:
                    _auditableHelper.SetDeletedProperties(entry.Entity);
                    break;
                case EntityState.Modified:
                    _auditableHelper.SetUpdatedProperties(entry.Entity);
                    break;
                default:
                    break;
            }
        }

        return base.SavingChanges(eventData, result);
    }
}