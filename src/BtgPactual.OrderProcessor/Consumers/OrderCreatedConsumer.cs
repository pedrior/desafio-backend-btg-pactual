using BtgPactual.OrderProcessor.Abstractions.Services;
using BtgPactual.OrderProcessor.Events;
using MassTransit;

namespace BtgPactual.OrderProcessor.Consumers;

public sealed class OrderCreatedConsumer(
    IOrderService orderService,
    ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
{
    public Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        logger.LogInformation("Consuming {Message} message", nameof(OrderCreatedEvent));
        
        return ProcessOrderAsync(context.Message);
    }

    private Task ProcessOrderAsync(OrderCreatedEvent e) => orderService.SaveOrderAsync(e);
}