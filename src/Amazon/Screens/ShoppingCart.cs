using Shared.Commands;

namespace Amazon.Screens;

public class ShoppingCart : Screen
{
    readonly IMessageSession messageSession;
    int quantity = 1;
    decimal price = 42;

    public ShoppingCart(IMessageSession messageSession)
    {
        this.messageSession = messageSession;
    }

    public override void Display()
    {
        Console.Clear();
        Console.WriteLine("###########################");
        Console.WriteLine("# Shopping cart           #");
        Console.WriteLine("###########################\n\n");
        Console.WriteLine("Title:                                               Price:      Quantity");
        Console.WriteLine("Patterns of Enterprise Application Architecture      $42.00" + quantity.ToString().PadLeft(14));
        Console.WriteLine("-------------------------------------------------------------------------");
        var totalPrice = "$" + (price * quantity).ToString();
        Console.WriteLine("                                                     Total:" + totalPrice.PadLeft(14));
        Console.WriteLine("Press + or - to increase/decrease quantity");
        Console.WriteLine("Press [ENTER] to continue...");
    }


    public override async Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Enter:
                await SendMessage(quantity);
                return FollowUpAction.ScreenIsDone;
                break;
            case ConsoleKey.OemPlus:
                if (quantity < 10) quantity++;
                Display();
                break;
            case ConsoleKey.OemMinus:
                if (quantity > 1) quantity--;
                Display();
                break;
            case ConsoleKey.Delete:
                return FollowUpAction.Exit;
        }

        return FollowUpAction.WaitForNewKeypress;
    }

    async Task SendMessage(int quanity)
    {
        var msg = new CreateOrder()
        {
            OrderId = Guid.NewGuid(),
            Quantity = quanity
        };

        await messageSession.Send(msg);
    }
}