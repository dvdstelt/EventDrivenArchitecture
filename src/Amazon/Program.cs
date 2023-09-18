using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                var endpointConfiguration = new EndpointConfiguration(Name).ApplyDefaultConfiguration();
                endpointConfiguration.SendOnly();
                endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                return endpointConfiguration;
            })
            .ConfigureServices(services => { services.AddHostedService<AmazonConsole>(); });
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