namespace BtgPactual.OrderProcessor.Abstractions.Endpoints;

public static class EndpointWebApplicationExtensions
{
    public static IApplicationBuilder UseEndpoints(this WebApplication app, RouteGroupBuilder? builder = null)
    {
        IEndpointRouteBuilder endpointRouteBuilder = builder is null ? app : builder;

        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.Map(endpointRouteBuilder);
        }

        return app;
    }
}