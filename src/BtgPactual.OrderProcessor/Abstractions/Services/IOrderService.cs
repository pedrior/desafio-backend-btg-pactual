using BtgPactual.OrderProcessor.Contracts.Responses;
using BtgPactual.OrderProcessor.Events;

namespace BtgPactual.OrderProcessor.Abstractions.Services;

public interface IOrderService
{
    Task SaveOrderAsync(OrderCreatedEvent e, CancellationToken cancellationToken = default);

    Task<OrderResponse?> GetOrderAsync(int orderId, CancellationToken cancellationToken = default);

    Task<PaginatedOrdersResponse> ListOrdersByCustomerAsync(
        int customerId,
        int page,
        int limit,
        CancellationToken cancellationToken = default);
}