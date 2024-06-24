using BtgPactual.OrderProcessor.Abstractions.Endpoints;
using BtgPactual.OrderProcessor.Abstractions.Persistence;
using BtgPactual.OrderProcessor.Abstractions.Services;
using BtgPactual.OrderProcessor.Consumers;
using BtgPactual.OrderProcessor.Options;
using BtgPactual.OrderProcessor.Persistence;
using BtgPactual.OrderProcessor.Services;
using MassTransit;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Match names like "order_id" with "OrderId"
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

builder.Services.AddOptions<RabbitMqOptions>()
    .Bind(builder.Configuration.GetSection(RabbitMqOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddScoped(_ =>
    new DbSession(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddMassTransit(config =>
{
    config.SetKebabCaseEndpointNameFormatter();

    config.UsingRabbitMq((context, configurator) =>
    {
        var options = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
        configurator.Host(options.Host, hostConfigurator =>
        {
            hostConfigurator.Username(options.Username);
            hostConfigurator.Password(options.Password);
        });

        configurator.ClearSerialization();
        configurator.UseRawJsonSerializer();

        configurator.ConfigureEndpoints(context);
    });

    config.AddConsumer<OrderCreatedConsumer>();
});

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddEndpoints(typeof(Program).Assembly);

var app = builder.Build();

app.UseEndpoints();

app.Run();