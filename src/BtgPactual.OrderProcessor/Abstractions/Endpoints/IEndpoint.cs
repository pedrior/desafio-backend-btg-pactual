namespace BtgPactual.OrderProcessor.Abstractions.Endpoints;

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder builder);
}