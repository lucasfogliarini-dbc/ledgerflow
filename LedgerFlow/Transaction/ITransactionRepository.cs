namespace LedgerFlow;

public interface ITransactionRepository : IRepository
{
    void Add<TEntity>(TEntity entity) where TEntity : Entity;
}
