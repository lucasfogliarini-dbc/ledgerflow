namespace LedgerFlow.Application.LedgerSummaries;
public record LedgerSummaryResponse(DateTime ReferenceDate, decimal Balance, decimal TotalCredits, decimal TotalDebits);