using BtgPactual.OrderProcessor.Abstractions.Endpoints;
using BtgPactual.OrderProcessor.Abstractions.Services;

namespace BtgPactual.OrderProcessor.Endpoints;

public sealed class GetOrder : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder) =>
        builder.MapGet("/orders/{id:int}", GetOrderAsync);

    private static async Task<IResult> GetOrderAsync(
        int id,
        HttpContext context,
        IOrderService orderService,
        CancellationToken cancellationToken)
    {
        var response = await orderService.GetOrderAsync(id, cancellationToken);
        return response is not null ? Results.Ok(response) : Results.NotFound();
    }
}