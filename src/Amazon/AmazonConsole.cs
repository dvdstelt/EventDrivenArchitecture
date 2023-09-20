using System.Net.Security;
using Amazon.Screens;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Amazon;

public class AmazonConsole : BackgroundService
{
    readonly IHostApplicationLifetime hostApplicationLifetime;

    Screen currentScreen;
    IEnumerable<Screen> screens;

    public AmazonConsole(IHostApplicationLifetime hostApplicationLifetime, IServiceProvider serviceProvider)
    {
        this.hostApplicationLifetime = hostApplicationLifetime;

        screens = serviceProvider.GetServices<Screen>();
        currentScreen = screens.Aggregate((x, y) => x.Sequence < y.Sequence ? x : y);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            // TODO: Fix that we don't move this object around through all screens
            var order = new Order();
            order.OrderId = Guid.NewGuid();

            Console.WriteLine("----------------------------------");
            Console.WriteLine("- Welcome to Amazon!             -");
            Console.WriteLine("----------------------------------\n");
            currentScreen.Order = order;
            currentScreen.Display();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!Console.KeyAvailable) continue;
                var userInput = Console.ReadKey(true);

                var result = await currentScreen.HandleKeyPress(userInput.Key);

                if (result == FollowUpAction.ScreenIsDone)
                {
                    currentScreen = screens.OrderBy(s => s.Sequence).First(s => s.Sequence > currentScreen.Sequence);
                    currentScreen.Order = order;
                    currentScreen.Display();
                }

                if (result == FollowUpAction.Exit)
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        hostApplicationLifetime.StopApplication();
    }
}