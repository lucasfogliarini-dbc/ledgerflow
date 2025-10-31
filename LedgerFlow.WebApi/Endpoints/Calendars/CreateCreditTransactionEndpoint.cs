using LedgerFlow.Application.Transactions;
using LedgerFlow.Application;

namespace LedgerFlow.WebApi.Endpoints.Transactions;

internal sealed class CreateCreditTransactionEndpoint : IEndpoint
{
    public async Task<IResult> CreateCreditTransactionAsync(
        CreateCreditTransactionRequest request,
        ICommandHandler<CreateCreditTransactionCommand> handler,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
            return Results.BadRequest("O corpo da requisição não pode ser nulo.");

        var command = new CreateCreditTransactionCommand(request.Value, request.Description);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok();
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost($"{Routes.Transactions}/credit", CreateCreditTransactionAsync)
           .WithTags(Routes.Transactions)
           .Produces(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status400BadRequest)
           .WithSummary("Cria uma nova transação de crédito.");
    }
}

internal sealed record CreateCreditTransactionRequest(decimal Value, string Description);
