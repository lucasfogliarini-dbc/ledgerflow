namespace LedgerFlow.Tests.Unit;

public class LedgerSummaryUnitTests
{
    [Fact(DisplayName = "AddTransaction_DeveAdicionarTransacaoCorretamente_QuandoTransacaoForValida")]
    public void AddTransaction_ShouldAddTransaction_WhenTransactionIsValid()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var summary = new LedgerSummary();
        var credit = Transaction.CreateCredit(500m, "Venda no cartão");
        var debit = Transaction.CreateDebit(100m, "Compra de material");

        // Act
        summary.AddTransaction(credit);
        summary.AddTransaction(debit);

        // Assert
        Assert.Equal(2, summary.Transactions.Count);
        Assert.Equal(500m, summary.TotalCredits);
        Assert.Equal(100m, summary.TotalDebits);
        Assert.Equal(400m, summary.FinalBalance);
    }

    [Theory(DisplayName = "AddTransaction_DeveLancarArgumentNullException_QuandoTransacaoForNula")]
    [InlineData(true)]
    public void AddTransaction_ShouldThrowArgumentNullException_WhenTransactionIsNull(bool dummy)
    {
        // Arrange
        var summary = new LedgerSummary();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => summary.AddTransaction(null!));
    }

    [Fact(DisplayName = "AddTransactions_DeveAdicionarVariasTransacoesCorretamente_QuandoListaForValida")]
    public void AddTransactions_ShouldAddMultipleTransactions_WhenListIsValid()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var summary = new LedgerSummary();

        var transactions = new List<Transaction>
        {
            Transaction.CreateCredit(100m, "Venda 1"),
            Transaction.CreateCredit(50m, "Venda 2"),
            Transaction.CreateDebit(30m, "Compra 1")
        };

        // Act
        summary.AddTransactions(transactions);

        // Assert
        Assert.Equal(3, summary.Transactions.Count);
        Assert.Equal(150m, summary.TotalCredits);
        Assert.Equal(30m, summary.TotalDebits);
        Assert.Equal(120m, summary.FinalBalance);
    }

    [Fact(DisplayName = "Adicionar transações deve lançar ArgumentNullException quando a lista for nula")]
    public void AddTransactions_ShouldThrowArgumentNullException_WhenListIsNull()
    {
        // Arrange
        var summary = new LedgerSummary();

        // Act e Assert
        Assert.Throws<ArgumentNullException>(() => summary.AddTransactions(null!));
    }
}
