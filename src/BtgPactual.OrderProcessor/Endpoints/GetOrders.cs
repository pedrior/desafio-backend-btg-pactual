using BtgPactual.OrderProcessor.Abstractions.Endpoints;
using BtgPactual.OrderProcessor.Abstractions.Services;
using BtgPactual.OrderProcessor.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BtgPactual.OrderProcessor.Endpoints;

public sealed class GetOrders : IEndpoint
{
    public void Map(IEndpointRouteBuilder builder) =>
        builder.MapGet("/customers/{customerId:int}/orders", ListCustomerOrdersAsync);

    private static async Task<IResult> ListCustomerOrdersAsync(
        [AsParameters] PageRequest request,
        [FromRoute] int customerId,
        HttpContext context,
        IOrderService orderService,
        CancellationToken cancellationToken)
    {
        var response = await orderService.ListOrdersByCustomerAsync(
            customerId,
            request.Page,
            request.Limit,
            cancellationToken);

        return Results.Ok(response);
    }
}