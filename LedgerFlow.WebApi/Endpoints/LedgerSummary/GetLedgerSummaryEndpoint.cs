using LedgerFlow.Application;
using LedgerFlow.Application.LedgerSummaries;

namespace LedgerFlow.WebApi.Endpoints;

internal sealed class GetLedgerSummaryEndpoint : IEndpoint
{
    public async Task<IResult> GetLedgerSummaryAsync(
        GetLedgerSummaryRequest request,
        IQueryHandler<GetLedgerSummaryQuery, GetLedgerSummaryResponse> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLedgerSummaryQuery(request.ReferenceDate);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet($"{Routes.LedgerSummaries}", GetLedgerSummaryAsync)
           .WithTags(Routes.LedgerSummaries)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest);
    }
}

internal sealed record GetLedgerSummaryRequest(DateTime ReferenceDate);
