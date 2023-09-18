using Shared.Commands;

namespace Amazon.Screens;

public class PaymentMethod : Screen
{
    readonly IMessageSession messageSession;

    public PaymentMethod(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("###########################");
        Console.WriteLine("# Payment method          #");
        Console.WriteLine("###########################\n");
        Console.WriteLine("Select your preferred payment method:\n");
        Console.WriteLine("[1] MasterCard ending in 1337 owned by Dennis van der Stelt");
        Console.WriteLine("[2] MasterCard ending in 0042 owned by Udi Dahan");
        Console.WriteLine("[3] Add new payment method (disabled)");
    }

    public override async Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                await SendMessage(Guid.Parse("ea7c7fed-ef1d-47ca-91e1-ef8e1b7edf20"));
                Order.PaymentMethod = "MasterCard ending in 1337 owned by Dennis van der Stelt";
                return FollowUpAction.ScreenIsDone;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                await SendMessage(Guid.Parse("3f53cc8f-0693-455d-900f-98789633527a"));
                Order.PaymentMethod = "MasterCard ending in 0042 owned by Udi Dahan";
                return FollowUpAction.ScreenIsDone;
            case ConsoleKey.Delete:
                return FollowUpAction.Exit;
        }

        return FollowUpAction.WaitForNewKeypress;
    }

    public override int Sequence => 4;

    async Task SendMessage(Guid paymentMethod)
    {
        var msg = new SubmitPayment()
        {
            OrderId = this.Order.OrderId,
            PaymentMethod = paymentMethod
        };

        await messageSession.Send(msg);
    }

}