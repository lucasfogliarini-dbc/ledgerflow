namespace LedgerFlow;

public interface ILedgerSummaryRepository : IRepository
{
    void Add(LedgerSummary ledgerSummary);
    Task<LedgerSummary> GetAsync(DateTime ReferenceDate, CancellationToken cancellationToken = default);
}
