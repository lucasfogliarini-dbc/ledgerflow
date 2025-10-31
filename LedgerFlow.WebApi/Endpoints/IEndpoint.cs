namespace LedgerFlow.WebApi;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
