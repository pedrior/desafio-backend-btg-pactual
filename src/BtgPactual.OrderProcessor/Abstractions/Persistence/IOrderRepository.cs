using BtgPactual.OrderProcessor.Entities;

namespace BtgPactual.OrderProcessor.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Order>> ListByCustomerAsync(
        int customerId,
        int page,
        int limit,
        CancellationToken cancellationToken = default);

    Task<int> CountByCustomerAsync(int customerId, CancellationToken cancellationToken = default);

    Task<decimal> GetTotalOrdersPriceByCustomerAsync(int customerId, CancellationToken cancellationToken = default);
    
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
}