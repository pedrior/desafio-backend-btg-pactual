namespace BtgPactual.OrderProcessor.Entities;

public sealed class Order
{
    public required int Id { get; init; }
    
    public required int CustomerId { get; init; }

    public required List<OrderItem> Items { get; init; } = [];
    
    public decimal TotalPrice { get; init; }
}