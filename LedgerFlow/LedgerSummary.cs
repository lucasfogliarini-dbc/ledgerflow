namespace LedgerFlow;

public class LedgerSummary
{
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public IList<Transaction> Transactions { get; private set; } = [];
    public decimal TotalCredits { get; private set; }
    public decimal TotalDebits { get; private set; }
    public decimal Balance => TotalCredits - TotalDebits;

    public void AddTransaction(Transaction transaction)
    {
        if (transaction is null)
            throw new ArgumentNullException(nameof(transaction));

        Transactions.Add(transaction);

        if (transaction.Type == TransactionType.Credit)
            TotalCredits += transaction.Value;
        else
            TotalDebits += transaction.Value;
    }

    public void AddTransactions(IEnumerable<Transaction> transactions)
    {
        if (transactions is null)
            throw new ArgumentNullException(nameof(transactions));

        foreach (var t in transactions)
            AddTransaction(t);
    }

    public override string ToString() => $"Consolidado para {CreatedAt:yyyy-MM-dd}: +{TotalCredits:C} -{TotalDebits:C} = {Balance:C}";
}
