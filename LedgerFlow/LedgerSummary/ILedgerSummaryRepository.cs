namespace LedgerFlow;

public interface ILedgerSummaryRepository : IRepository
{
    void Add<TEntity>(TEntity entity) where TEntity : Entity;
}
