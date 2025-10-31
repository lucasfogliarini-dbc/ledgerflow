namespace LedgerFlow;

public class Transaction : IAuditable
{
    public TransactionType Type { get; }
    public decimal Value { get; }
    public string Description { get; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime UpdatedAt { get; private set; } = DateTime.Now;

    private Transaction(TransactionType type, decimal value, string description)
    {
        Type = type;
        Value = value;
        Description = description;
    }

    /// <summary>
    /// Cria uma transação de crédito.
    /// </summary>
    public static Transaction CreateCredit(decimal value, string description)
    {
        if (value <= 0)
            throw new ArgumentException("O valor do crédito deve ser maior que zero.", nameof(value));

        return new Transaction(
            type: TransactionType.Credit,
            value: value,
            description: description
        );
    }

    /// <summary>
    /// Cria uma transação de débito.
    /// </summary>
    public static Transaction CreateDebit(decimal value, string description)
    {
        if (value <= 0)
            throw new ArgumentException("O valor do débito deve ser maior que zero.", nameof(value));

        return new Transaction(
            type: TransactionType.Debit,
            value: value,
            description: description
        );
    }

    public override string ToString() =>  $"{Type} {Value:C} - {Description} ({CreatedAt:yyyy-MM-dd HH:mm})";
}
