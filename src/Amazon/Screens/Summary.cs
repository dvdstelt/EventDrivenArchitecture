using Shared.Commands;

namespace Amazon.Screens;

public class Summary : Screen
{
    readonly IMessageSession messageSession;

    public Summary(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("###########################");
        Console.WriteLine("# Summary           #");
        Console.WriteLine("###########################\n\n");
        Console.WriteLine("Title:                                                         Total:");
        var totalPrice = "$" + (Order.Price * Order.Quantity).ToString();
        Console.WriteLine(
            $"{Order.Quantity} x Patterns of Enterprise Application Architecture" + totalPrice.PadLeft(16));
        Console.WriteLine("-------------------------------------------------------------------------");
        Console.WriteLine(totalPrice.PadLeft(67));
        Console.WriteLine("\nShipping method: " + Order.ShippingMethod);
        Console.WriteLine("Payment method: " + Order.PaymentMethod);
        Console.WriteLine("Press [ENTER] to finalize the order...");
    }

    public override async Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Enter:
                await SendMessage();
                return FollowUpAction.Exit;
            case ConsoleKey.Delete:
                return FollowUpAction.Exit;
        }

        return FollowUpAction.WaitForNewKeypress;
    }

    public override int Sequence => 5;

    async Task SendMessage()
    {
        var msg = new AcceptOrder()
        {
            OrderId = this.Order.OrderId,
            Quantity = Order.Quantity
        };

        await messageSession.Send(msg);
    }
}