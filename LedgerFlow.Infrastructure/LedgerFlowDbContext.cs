using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LedgerFlow.Infrastructure;

internal class LedgerFlowDbContext(DbContextOptions options) : DbContext(options), ICommitScope
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(thisAssembly);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    public int Commit()
    {
        return base.SaveChanges();
    }
}
