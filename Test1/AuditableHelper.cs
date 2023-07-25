
using Microsoft.Extensions.DependencyInjection;

namespace Test1;

//Can be registered as transient
public class AuditableHelper : IAuditableHelper
{
    private IAppUser currentUser;

    public AuditableHelper(IServiceScopeFactory serviceScopeFactory)
    {
        using (var serviceScope = serviceScopeFactory.CreateScope())
        {
             currentUser = serviceScope.ServiceProvider.GetRequiredService<IAppUser>();
        }

    }

    public void SetCreatedProperties(IAuditable auditable)
    {
        auditable.CreatedDateUtc = DateTime.UtcNow;
        auditable.CreatedUser = GetUserIdentifierForAudit(currentUser);
    }


    public void SetDeletedProperties(IAuditable auditable)
    {
        auditable.DeletedDateUtc = DateTime.UtcNow;
        auditable.DeletedUser = GetUserIdentifierForAudit(currentUser);
        auditable.IsDeleted = true;
    }


    public void SetUpdatedProperties(IAuditable auditable)
    {
        auditable.UpdatedDateUtc = DateTime.UtcNow;
        auditable.UpdatedUser = GetUserIdentifierForAudit(currentUser);
    }

    private string GetUserIdentifierForAudit(IAppUser currentUser)
    {
        return  currentUser.Email ?? currentUser.UserId.ToString();
    }
}