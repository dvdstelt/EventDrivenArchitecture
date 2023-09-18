using System.Net.Security;
using Amazon.Screens;
using Microsoft.Extensions.Hosting;

namespace Amazon;

public class AmazonConsole : BackgroundService
{
    readonly IHostApplicationLifetime hostApplicationLifetime;

    Screen currentScreen;
    List<Screen> screens;

    public AmazonConsole(IHostApplicationLifetime hostApplicationLifetime)
    {
        this.hostApplicationLifetime = hostApplicationLifetime;
        currentScreen = new ShoppingCart();
        screens = new List<Screen>() { currentScreen, new ShippingAddress() };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Console.WriteLine("----------------------------------");
            Console.WriteLine("- Welcome to Amazon!             -");
            Console.WriteLine("----------------------------------\n");
            currentScreen.Display();

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!Console.KeyAvailable) continue;
                var userInput = Console.ReadKey(true);

                var result = currentScreen.HandleKeyPress(userInput.Key).Result;

                if (result == FollowUpAction.ScreenIsDone)
                {
                    var nextScreenIndex = screens.IndexOf(currentScreen) + 1;
                    if (nextScreenIndex >= screens.Count)
                        throw new ApplicationException("There's no next screen available...");
                    currentScreen = screens[nextScreenIndex];
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

        //await base.StopAsync(stoppingToken);
        hostApplicationLifetime.StopApplication();
    }
}