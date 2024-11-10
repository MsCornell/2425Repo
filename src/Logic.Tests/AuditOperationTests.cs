using System;

namespace Logic.Tests;

public class AuditOperationTests
{
    private AuditOperationRepository repository = new("http://localhost:5000/api/Audit_Operation");

    [Fact]
    // create audit operation
    public async Task CreateAuditOperation_NoError()
    {
        // arrange
        var auditOperation = new AuditOperation { Name = "Test" };
        // act
        var created = await repository.CreateAuditOperationAsync(auditOperation);
        await repository.DeleteAuditOperationAsync(created.Id);
        // assert
        Assert.NotNull(created);
        Assert.Equal(auditOperation.Name, created.Name);
    }

    // update audit operation
    [Fact]
    public async Task UpdateAuditOperation_NoError()
    {
        // arrange
        var auditOperation = new AuditOperation { Name = "Test" };
        var created = await repository.CreateAuditOperationAsync(auditOperation);
        created.Name = "Test2";
        // act
        var updated = await repository.UpdateAuditOperationAsync(created);
        await repository.DeleteAuditOperationAsync(updated.Id);
        // assert
        Assert.NotNull(updated);
        Assert.Equal(created.Name, updated.Name);
    }
}
