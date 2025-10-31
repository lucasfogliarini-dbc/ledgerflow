namespace LedgerFlow.Infrastructure.Repositories;

internal class LedgerSummaryRepository(LedgerFlowDbContext dbContext) : Repository(dbContext), ILedgerSummaryRepository
{
}
