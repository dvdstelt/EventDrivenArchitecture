using Amazon.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.Commands;
using Shared.NServiceBus;

namespace Amazon;

internal static class Program
{
    const string Name = "Amazon";

    static void Main(string[] args)
    {
        Console.Title = Name;
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseConsoleLifetime()
            .ConfigureLogging(logging => { logging.AddConsole(); })
            .UseNServiceBus(ctx =>
            {
                var endpointConfiguration =
                    new EndpointConfiguration(Name).ApplyDefaultConfiguration(s =>
                    {
                        s.RouteToEndpoint(typeof(AcceptOrder), "Amazon.Sales");
                        s.RouteToEndpoint(typeof(SubmitPayment), "Amazon.Finance");
                        s.RouteToEndpoint(typeof(SubmitShippingAddress), "Amazon.Shipping");
                        s.RouteToEndpoint(typeof(SubmitShippingMethod), "Amazon.Shipping");
                    });
                endpointConfiguration.SendOnly();
                endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                return endpointConfiguration;
            })
            .ConfigureServices(services =>
            {
                services.AddHostedService<AmazonConsole>();
                services.AddSingleton<Screen, ShoppingCart>();
                services.AddSingleton<Screen, ShippingAddress>();
                services.AddSingleton<Screen, ShippingMethod>();
                services.AddSingleton<Screen, PaymentMethod>();
                services.AddSingleton<Screen, Summary>();
            });
    }

    static async Task OnCriticalError(ICriticalErrorContext context, CancellationToken cancellationToken)
    {
        try
        {
            await context.Stop(cancellationToken);
        }
        finally
        {
            Environment.FailFast($"Critical error, shutting down: {context.Error}", context.Exception);
        }
    }
}