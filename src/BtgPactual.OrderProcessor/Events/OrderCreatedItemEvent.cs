using System.Text.Json.Serialization;

namespace BtgPactual.OrderProcessor.Events;

public sealed record OrderCreatedItemEvent
{
    [JsonPropertyName("produto")]
    public string Product { get; init; } = string.Empty;

    [JsonPropertyName("quantidade")]
    public int Quantity { get; init; }

    [JsonPropertyName("preco")]
    public decimal Price { get; init; }
}