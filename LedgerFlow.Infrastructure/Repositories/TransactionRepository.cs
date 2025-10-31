namespace LedgerFlow.Infrastructure.Repositories;

internal class TransactionRepository(LedgerFlowDbContext dbContext) : Repository(dbContext), ITransactionRepository
{
}
