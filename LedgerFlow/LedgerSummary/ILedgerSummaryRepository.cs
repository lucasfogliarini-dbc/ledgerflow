namespace LedgerFlow;

public interface ILedgerSummaryRepository : IRepository
{
    void Add(LedgerSummary ledgerSummary);
    Task<IEnumerable<LedgerSummary>> GetAsync(DateTime ReferenceDate, CancellationToken cancellationToken = default);
}
