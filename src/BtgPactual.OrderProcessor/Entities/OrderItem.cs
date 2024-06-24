namespace BtgPactual.OrderProcessor.Entities;

public sealed class OrderItem
{
    public required int OrderId { get; init; }
    
    public required string Product { get; init; }
    
    public required int Quantity { get; init; }
    
    public required decimal Price { get; init; }
}