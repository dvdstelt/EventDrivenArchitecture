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
        Console.WriteLine("# Shipping address        #");
        Console.WriteLine("###########################\n");
        Console.WriteLine("Select address:\n");
        Console.WriteLine("[1] Dennis van der Stelt, Van Zandvlietplein 1, Rotterdam");
        Console.WriteLine("[2] Dennis van der Stelt, Parkkade 1, Rotterdam");
    }

    public override async Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                await SendMessage(Guid.Parse("5306098a-1958-4cc6-99ad-b5354d711887"));
                Order.ShippingAddress = "Van Zandvlietplein 1, Rotterdam";
                return FollowUpAction.ScreenIsDone;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                await SendMessage(Guid.Parse("4002f279-8030-492a-b54b-129838689b08"));
                Order.ShippingAddress = "Parkkade 1, Rotterdam";
                return FollowUpAction.ScreenIsDone;
            case ConsoleKey.Delete:
                return FollowUpAction.Exit;
        }

        return FollowUpAction.WaitForNewKeypress;
    }

    public override int Sequence => 2;

    async Task SendMessage(Guid address)
    {
        var msg = new SubmitShippingAddress()
        {
            OrderId = Guid.NewGuid(),
            Address = address
        };

        await messageSession.Send(msg);
    }
}