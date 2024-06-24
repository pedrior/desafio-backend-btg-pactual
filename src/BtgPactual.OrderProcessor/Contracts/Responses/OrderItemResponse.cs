namespace BtgPactual.OrderProcessor.Contracts.Responses;

public sealed record OrderItemResponse
{
    public string Product { get; init; } = null!;

    public int Quantity { get; init; }

    public decimal Price { get; init; }
}