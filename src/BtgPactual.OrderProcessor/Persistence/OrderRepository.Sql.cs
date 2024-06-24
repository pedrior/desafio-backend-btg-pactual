namespace BtgPactual.OrderProcessor.Persistence;

public sealed partial class OrderRepository
{
    private const string GetOrderSql = """
                                       SELECT *
                                       FROM orders
                                       WHERE id = @id
                                       LIMIT 1;
                                       """;

    private const string GetOrderByCustomerSql = """
                                                 SELECT *
                                                 FROM Orders
                                                 WHERE customer_id = @customerId
                                                 ORDER BY Id
                                                 OFFSET @offset ROWS
                                                 FETCH NEXT @limit ROWS ONLY
                                                 """;

    private const string GetOrderItemsSql = """
                                            SELECT *
                                            FROM order_items
                                            WHERE order_id = @id
                                            ORDER BY product;
                                            """;

    private const string CountOrderByCustomerSql = """
                                                    SELECT COUNT(*)
                                                    FROM orders
                                                    WHERE customer_id = @customerId;
                                                   """;

    private const string GetTotalOrdersPriceByCustomer = """
                                                         SELECT SUM(o.total_price)
                                                         FROM Orders o
                                                         WHERE o.customer_id = @customerId
                                                         """;

    private const string InsertOrderSql = """
                                           INSERT INTO orders (id, customer_id, total_price)
                                           VALUES (@Id, @CustomerId, @TotalPrice);
                                          """;

    private const string InsertOrderItemSql = """
                                               INSERT INTO order_items (order_id, product, quantity, price)
                                               VALUES (@OrderId, @Product, @Quantity, @Price);
                                              """;

    private const string CreateTableSql = """
                                          CREATE TABLE IF NOT EXISTS orders (
                                            id INTEGER PRIMARY KEY,
                                            customer_id INTEGER NOT NULL,
                                            total_price DECIMAL(10, 2) NOT NULL);

                                          CREATE TABLE IF NOT EXISTS order_items (
                                            order_id INTEGER NOT NULL,
                                            product VARCHAR(150) NOT NULL,
                                            quantity INTEGER NOT NULL,
                                            price DECIMAL(10, 2) NOT NULL,
                                               
                                          FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE);

                                          CREATE INDEX IF NOT EXISTS idx_orders_customer_id ON orders (customer_id);
                                          CREATE INDEX IF NOT EXISTS idx_order_items_order_id ON order_items (order_id);
                                          """;
}