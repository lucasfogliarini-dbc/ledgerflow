using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;

namespace LedgerFlow.Application.LedgerSummaries;

public class GetLedgerSummariesQueryHandler(ILedgerSummaryRepository ledgerSummaryRepository, ILogger<GetLedgerSummariesQueryHandler> logger) : IQueryHandler<GetLedgerSummariesQuery, GetLedgerSummariesResponse>
{
    public async Task<Result<GetLedgerSummariesResponse>> HandleAsync(GetLedgerSummariesQuery query, CancellationToken cancellationToken = default)
    {
        if (query is null)
            return Result.Failure<GetLedgerSummariesResponse>("A query não pode ser nula.");

        var ledgerSummaries = await ledgerSummaryRepository.GetAsync(query.ReferenceDate, cancellationToken);

        if(!ledgerSummaries.Any())
            return Result.Failure<GetLedgerSummariesResponse>("Nenhum relatório consolidado foi encontrado.");

        var referenceDate = ledgerSummaries.FirstOrDefault().ReferenceDate.Date;
        var ledgerSummariesResponse = ledgerSummaries.Select(e => new LedgerSummaryResponse(e.ReferenceDate, e.Balance, e.TotalCredits, e.TotalDebits));
        return Result.Success(new GetLedgerSummariesResponse(referenceDate, ledgerSummariesResponse));
    }
}

public record GetLedgerSummariesQuery(DateTime ReferenceDate) : IQuery<GetLedgerSummariesResponse>;
public record GetLedgerSummariesResponse(DateTime ReferenceDate, IEnumerable<LedgerSummaryResponse> LedgerSummaries);
public record LedgerSummaryResponse(DateTime ReferenceDate, decimal Balance, decimal TotalCredits, decimal TotalDebits);
