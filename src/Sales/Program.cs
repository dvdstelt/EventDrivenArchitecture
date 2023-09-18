using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.NServiceBus;

namespace Finance;

internal static class Program
{
    const string Name = "Amazon.Sales";

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
                endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);

                return endpointConfiguration;
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