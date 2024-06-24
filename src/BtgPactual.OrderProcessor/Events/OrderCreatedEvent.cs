using System.Text.Json.Serialization;

namespace BtgPactual.OrderProcessor.Events;

public sealed record OrderCreatedEvent
{
    [JsonPropertyName("codigoPedido")]
    public int OrderId { get; init; }

    [JsonPropertyName("codigoCliente")]
    public int CustomerId { get; init; }

    [JsonPropertyName("itens")]
    public IEnumerable<OrderCreatedItemEvent> Items { get; init; } = [];
}