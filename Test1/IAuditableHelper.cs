namespace Test1;

public interface IAuditableHelper
{
    void SetCreatedProperties(IAuditable auditable);
    void SetDeletedProperties(IAuditable auditable);
    void SetUpdatedProperties(IAuditable auditable);
}
