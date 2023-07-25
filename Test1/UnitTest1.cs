using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using System.Text.Json;
using FluentAssertions.Extensions;

namespace Test1;

public class UnitTest1 : TestWithSqlite
{
    [Fact]
    public async Task DatabaseIsAvailableAndCanBeConnectedTo()
    {
        (await DbContext.Database.CanConnectAsync()).Should().BeTrue();
    }

    [Fact]
    public void TableShouldGetCreated()
    {
        DbContext.Workspaces!.Any().Should().BeFalse();
    }


    [Fact]
    public async Task WhenInserted_AuditableFieldsShouldBeCreated()
    {
        var workspace = new Workspace { Id = 1, TenantBrandId = Guid.NewGuid()};
        DbContext.Workspaces.Add(workspace);
        await DbContext.SaveChangesAsync();
        var result = DbContext.Workspaces.Single(x => x.Id == 1);

        result.CreatedUser.Should().Be("fkucuk@email.com");
        result.CreatedDateUtc.Should().BeCloseTo(DateTime.UtcNow, 2.Seconds());
        result.IsDeleted.Should().Be(false);
        result.DeletedDateUtc.Should().Be(null);
        result.DeletedUser.Should().Be(null);
        result.UpdatedDateUtc.Should().Be(null);
        result.UpdatedUser.Should().Be(null);
    }

    [Fact]
    public async Task WhenUpdated_AuditableFieldsShouldBeConsistent()
    {
        var workspace = new Workspace { Id = 2, TenantBrandId = Guid.NewGuid() };
        DbContext.Workspaces.Add(workspace);
        var expetedCreatedDateUtc = DateTime.UtcNow;
        await DbContext.SaveChangesAsync();
        var result = DbContext.Workspaces.Single(x => x.Id == 2);

        result.TenantBrandId = Guid.NewGuid();

        var expetedUpdatedDateUtc = DateTime.UtcNow;
        await DbContext.SaveChangesAsync();

        result?.CreatedUser.Should().Be("fkucuk@email.com");
        result?.CreatedDateUtc.Should().BeCloseTo(expetedCreatedDateUtc, 2.Seconds());
        result?.IsDeleted.Should().Be(false);
        result?.DeletedDateUtc.Should().Be(null);
        result?.DeletedUser.Should().Be(null);
        result?.UpdatedDateUtc.Should().BeCloseTo(expetedUpdatedDateUtc, 2.Seconds());
        result?.UpdatedUser.Should().Be("fkucuk@email.com");
    }

    [Fact(Skip = "Soft Delete will be Implemented")]
    public async Task WhenDeleted_AuditableFieldsShouldBeConsistent()
    {
        var workspace = new Workspace { Id = 3, TenantBrandId = Guid.NewGuid() };
        DbContext.Workspaces.Add(workspace);
        var expetedCreatedDateUtc = DateTime.UtcNow;
        await DbContext.SaveChangesAsync();
        var result = DbContext.Workspaces.Single(x => x.Id == 2);

        result.TenantBrandId = Guid.NewGuid();

        var expetedUpdatedDateUtc = DateTime.UtcNow;
        await DbContext.SaveChangesAsync();

        result?.CreatedUser.Should().Be("fkucuk@email.com");
        result?.CreatedDateUtc.Should().BeCloseTo(expetedCreatedDateUtc, 2.Seconds());
        result?.IsDeleted.Should().Be(false);
        result?.DeletedDateUtc.Should().Be(null);
        result?.DeletedUser.Should().Be(null);
        result?.UpdatedDateUtc.Should().BeCloseTo(expetedUpdatedDateUtc, 2.Seconds());
        result?.UpdatedUser.Should().Be("fkucuk@email.com");
    }


}
