namespace LedgerFlow.WebApi;

public interface IEndpoint
{
    IEndpointConventionBuilder MapEndpoint(IEndpointRouteBuilder app);
}