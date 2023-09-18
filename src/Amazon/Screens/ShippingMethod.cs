using Shared.Commands;

namespace Amazon.Screens;

public class ShippingMethod : Screen
{
    readonly IMessageSession messageSession;

    public ShippingMethod(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("###########################");
        Console.WriteLine("# Shipping method         #");
        Console.WriteLine("###########################\n");
        Console.WriteLine("Select how to ship your order:\n");
        Console.WriteLine("[1] $5  - Standard shipping");
        Console.WriteLine("[2] $10 - Expedited shipping");
        Console.WriteLine("[3] $20 - Priority shipping");
    }

    public override async Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        var shipmentMethod = string.Empty;
        Guid shippingMethodId;
        switch (key)
        {
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                shipmentMethod = "Standard Shipping";
                shippingMethodId = Guid.Parse("a5b24d31-e4bc-4fae-9ff4-a40cc68612bb");
                goto case ConsoleKey.NoName;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                shipmentMethod = "Expedited Shippping";
                shippingMethodId = Guid.Parse("ff3c5c9f-13f8-44d8-8523-9e0fc44b2765");
                goto case ConsoleKey.NoName;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                shipmentMethod = "Priority Shipping";
                shippingMethodId = Guid.Parse("fb717ff2-5ae8-42e8-878d-b7f8f4dd44a0");
                goto case ConsoleKey.NoName;
            case ConsoleKey.NoName: // Ouch, nasty cheat ;-)
                await SendMessage(shipmentMethod);
                Order.ShippingMethod = shipmentMethod;
                return FollowUpAction.ScreenIsDone;
            case ConsoleKey.Delete:
                return FollowUpAction.Exit;
        }

        return FollowUpAction.WaitForNewKeypress;
    }

    public override int Sequence => 3;

    async Task SendMessage(string shippingMethod)
    {
        var msg = new SubmitShippingMethod()
        {
            OrderId = this.Order.OrderId,
            ShippingMethod = shippingMethod
        };

        await messageSession.Send(msg);
    }

}