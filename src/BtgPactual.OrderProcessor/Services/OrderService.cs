using BtgPactual.OrderProcessor.Abstractions.Persistence;
using BtgPactual.OrderProcessor.Abstractions.Services;
using BtgPactual.OrderProcessor.Contracts.Responses;
using BtgPactual.OrderProcessor.Entities;
using BtgPactual.OrderProcessor.Events;

namespace BtgPactual.OrderProcessor.Services;

public sealed class OrderService(IOrderRepository orderRepository) : IOrderService
{
    public Task SaveOrderAsync(OrderCreatedEvent e, CancellationToken cancellationToken = default)
    {
        var orderItems = e.Items.Select(i => new OrderItem
        {
            OrderId = e.OrderId,
            Product = i.Product,
            Quantity = i.Quantity,
            Price = i.Price
        }).ToList();

        var order = new Order
        {
            Id = e.OrderId,
            CustomerId = e.CustomerId,
            Items = orderItems,
            TotalPrice = orderItems.Sum(i => i.Price * i.Quantity)
        };

        return orderRepository.AddAsync(order, cancellationToken);
    }

    public async Task<OrderResponse?> GetOrderAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetAsync(orderId, cancellationToken);
        return order is not null ? ConvertToOrderResponse(order) : null;
    }

    public async Task<PaginatedOrdersResponse> ListOrdersByCustomerAsync(
        int customerId,
        int page,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var total = await orderRepository.CountByCustomerAsync(customerId, cancellationToken);
        if (total is 0)
        {
            return PaginatedOrdersResponse.Empty(page, limit);
        }

        var orders = await orderRepository.ListByCustomerAsync(customerId, page, limit, cancellationToken);
        var totalOrdersPrice = await orderRepository.GetTotalOrdersPriceByCustomerAsync(customerId, cancellationToken);

        return new PaginatedOrdersResponse
        {
            Page = page,
            Limit = limit,
            TotalOrdersPrice = totalOrdersPrice,
            TotalOrders = total,
            Items = orders.Select(ConvertToOrderResponse),
        };
    }

    private static OrderResponse ConvertToOrderResponse(Order order) => new()
    {
        Id = order.Id,
        CustomerId = order.CustomerId,
        TotalPrice = order.TotalPrice,
        Items = order.Items.Select(oi => new OrderItemResponse
        {
            Product = oi.Product,
            Quantity = oi.Quantity,
            Price = oi.Price
        })
    };
}