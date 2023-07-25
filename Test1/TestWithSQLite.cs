using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.Extensions.DependencyInjection;
namespace Test1;

public abstract class TestWithSqlite : IDisposable
{
    private const string InMemoryConnectionString = "DataSource=:memory:";
    //private const string InMemoryConnectionString = @"DataSource=test.db";
    private readonly SqliteConnection _connection;

    protected AppDbContext DbContext;

    protected TestWithSqlite()
    {
        _connection = new SqliteConnection(InMemoryConnectionString);
        _connection.Open();
        DbContext = CreateDbContext(_connection);
    }

    private AppDbContext CreateDbContext(SqliteConnection connection)
    {
        
        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider
            .Setup(x => x.GetService(typeof(IAppUser)))
            .Returns(GetDummmyUser());

        var serviceScope = new Mock<IServiceScope>();
        serviceScope.Setup(x => x.ServiceProvider).Returns(serviceProvider.Object);

        var serviceScopeFactory = new Mock<IServiceScopeFactory>();
        serviceScopeFactory
            .Setup(x => x.CreateScope())
            .Returns(serviceScope.Object);


        IAuditableHelper auditableHelper = new AuditableHelper(serviceScopeFactory.Object);

        var interceptor = new AuditableEntitiesInterceptor(auditableHelper);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .AddInterceptors(interceptor)
            .Options;
        var dbContext = new AppDbContext(options);
        dbContext.Database.EnsureCreated();
        return dbContext;
    }

    private IAppUser GetDummmyUser()
    {
        return new AppUser("fkucuk", Guid.NewGuid(), "fkucuk@email.com");
    }

    internal void RecreateDbContext()
    {
        var previousDbContext = DbContext;
        DbContext = CreateDbContext(_connection);
        previousDbContext.Dispose();
    }

    public void Dispose()
    {
        DbContext?.Dispose();
        _connection.Dispose();
    }
}