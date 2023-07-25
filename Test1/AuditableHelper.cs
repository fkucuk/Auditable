
using Microsoft.Extensions.DependencyInjection;

namespace Test1;

//Can be registered as transient
public class AuditableHelper : IAuditableHelper
{

    private readonly IServiceScope _serviceScope;

    public AuditableHelper(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScope = serviceScopeFactory.CreateScope();

    }

    public void SetCreatedProperties(IAuditable auditable)
    {
        var appUser = _serviceScope.ServiceProvider.GetRequiredService<IAppUser>();
        auditable.CreatedDateUtc = DateTime.UtcNow;
        auditable.CreatedUser = GetUserIdentifierForAudit(appUser);
    }


    public void SetDeletedProperties(IAuditable auditable)
    {
        var appUser = _serviceScope.ServiceProvider.GetRequiredService<IAppUser>();
        auditable.DeletedDateUtc = DateTime.UtcNow;
        auditable.DeletedUser = GetUserIdentifierForAudit(appUser);
        auditable.IsDeleted = true;
    }


    public void SetUpdatedProperties(IAuditable auditable)
    {
        var appUser = _serviceScope.ServiceProvider.GetRequiredService<IAppUser>();
        auditable.UpdatedDateUtc = DateTime.UtcNow;
        auditable.UpdatedUser = GetUserIdentifierForAudit(appUser);
    }

    private string GetUserIdentifierForAudit(IAppUser currentUser)
    {
        return  currentUser.Email ?? currentUser.UserId.ToString();
    }
}