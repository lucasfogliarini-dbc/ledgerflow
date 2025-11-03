using LedgerFlow.Application;
using LedgerFlow.Application.LedgerSummaries;
using Microsoft.AspNetCore.Mvc;

namespace LedgerFlow.LedgerSummaries.WebApi.Endpoints;

internal sealed class GetLedgerSummariesEndpoint : IEndpoint
{
    public async Task<IResult> GetLedgerSummariesAsync(
        [FromBody] GetLedgerSummaryRequest request,
        IQueryHandler<GetLedgerSummariesQuery, GetLedgerSummariesResponse> handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetLedgerSummariesQuery(request.ReferenceDate);
        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    public IEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder app)
    {
        return app.MapGet($"{Routes.LedgerSummaries}", GetLedgerSummariesAsync)
           .WithTags(Routes.LedgerSummaries)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest);
    }
}

internal sealed record GetLedgerSummaryRequest(DateTime ReferenceDate);
