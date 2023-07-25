
using Microsoft.Extensions.DependencyInjection;

namespace Test1;

//Can be registered as transient
public class AuditableHelper : IAuditableHelper
{

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuditableHelper(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory; 

    }

    public void SetCreatedProperties(IAuditable auditable)
    {
        auditable.CreatedDateUtc = DateTime.UtcNow;
        auditable.CreatedUser = GetUserIdentifierForAudit();
    }


    public void SetDeletedProperties(IAuditable auditable)
    {
        auditable.DeletedDateUtc = DateTime.UtcNow;
        auditable.DeletedUser = GetUserIdentifierForAudit();
        auditable.IsDeleted = true;
    }


    public void SetUpdatedProperties(IAuditable auditable)
    {
        auditable.UpdatedDateUtc = DateTime.UtcNow;
        auditable.UpdatedUser = GetUserIdentifierForAudit();
    }

    private string GetUserIdentifierForAudit()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var appUser = serviceScope.ServiceProvider.GetRequiredService<IAppUser>();
        return appUser.Email ?? appUser.UserId.ToString();
    }
}