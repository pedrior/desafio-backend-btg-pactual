namespace BtgPactual.OrderProcessor.Contracts.Responses;

public sealed record OrderResponse
{
    public int Id { get; init; }

    public int CustomerId { get; init; }

    public decimal TotalPrice { get; init; }
    
    public IEnumerable<OrderItemResponse> Items { get; init; } = [];
}