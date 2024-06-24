namespace BtgPactual.OrderProcessor.Contracts.Responses;

public sealed record PaginatedOrdersResponse
{
    public required int Page { get; init; }

    public required int Limit { get; init; }

    public required int TotalOrders { get; init; }
    
    public required decimal TotalOrdersPrice { get; init; }

    public int TotalPages => (int)Math.Ceiling(TotalOrders / (double)Limit);

    public required IEnumerable<OrderResponse> Items { get; init; } = [];

    public static PaginatedOrdersResponse Empty(int page, int limit) => new()
    {
        Page = page,
        Limit = limit,
        TotalOrders = 0,
        Items = [],
        TotalOrdersPrice = 0m
    };
}