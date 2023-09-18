using Shared.Commands;

namespace Amazon.Screens;

public class ShippingAddress : Screen
{
    readonly IMessageSession messageSession;

    public ShippingAddress(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("###########################");
        Console.WriteLine("# Shopping cart           #");
        Console.WriteLine("###########################\n");
        Console.WriteLine("Select address:\n");
        Console.WriteLine("[1] Dennis van der Stelt, Van Zandvlietplein 1, Rotterdam");
        Console.WriteLine("[2] Dennis van der Stelt, Parkkade 1, Rotterdam");
    }


    public override async Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Oem1:
                await SendMessage(1);
                return FollowUpAction.ScreenIsDone;
                break;
            case ConsoleKey.Oem2:
                await SendMessage(2);
                return FollowUpAction.ScreenIsDone;
                break;
            case ConsoleKey.Delete:
                return FollowUpAction.Exit;
        }

        return FollowUpAction.WaitForNewKeypress;
    }

    async Task SendMessage(int address)
    {
        var msg = new SubmitShippingAddress()
        {
            OrderId = Guid.NewGuid(),
            Address = Guid.NewGuid()
        };

        await messageSession.Send(msg);
    }
}