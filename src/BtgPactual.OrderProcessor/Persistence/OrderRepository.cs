using BtgPactual.OrderProcessor.Abstractions.Persistence;
using BtgPactual.OrderProcessor.Entities;
using Dapper;

namespace BtgPactual.OrderProcessor.Persistence;

public sealed partial class OrderRepository : IOrderRepository
{
    private readonly DbSession session;

    public OrderRepository(DbSession session)
    {
        this.session = session;

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            EnsureTableCreatedAsync();
        }
    }

    public async Task<Order?> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await session.Connection.QuerySingleOrDefaultAsync<Order?>(
            sql: GetOrderSql,
            param: new { id });

        if (order is null)
        {
            return null;
        }

        var orderItems = await session.Connection.QueryAsync<OrderItem>(
            sql: GetOrderItemsSql,
            param: new { id });

        order.Items.AddRange(orderItems);
        return order;
    }

    public async Task<int> CountByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await session.Connection.ExecuteScalarAsync<int>(
            sql: CountOrderByCustomerSql,
            param: new { customerId });
    }

    public async Task<decimal> GetTotalOrdersPriceByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await session.Connection.ExecuteScalarAsync<decimal>(
            sql: GetTotalOrdersPriceByCustomer,
            param: new { customerId });
    }

    public async Task<IReadOnlyCollection<Order>> ListByCustomerAsync(
        int customerId,
        int page,
        int limit,
        CancellationToken cancellationToken = default)
    {
        if (page < 1 || limit < 1)
        {
            return [];
        }

        var orders = (await session.Connection.QueryAsync<Order>(
                sql: GetOrderByCustomerSql,
                param: new
                {
                    customerId,
                    Limit = limit,
                    Offset = (page - 1) * limit
                }))
            .ToList();

        foreach (var order in orders)
        {
            var orderItems = await session.Connection.QueryAsync<OrderItem>(
                sql: GetOrderItemsSql,
                param: new { order.Id });

            order.Items.AddRange(orderItems);
        }

        return orders;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        session.BeginTransaction();

        try
        {
            await session.Connection.ExecuteAsync(
                sql: InsertOrderSql,
                param: order,
                transaction: session.Transaction);

            await session.Connection.ExecuteAsync(
                sql: InsertOrderItemSql,
                param: order.Items,
                transaction: session.Transaction);

            session.CommitTransaction();
        }
        catch
        {
            session.RollbackTransaction();
            throw;
        }
    }

    private void EnsureTableCreatedAsync() => session.Connection.Execute(CreateTableSql);
}