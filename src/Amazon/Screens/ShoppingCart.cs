using Shared.Commands;

namespace Amazon.Screens;

public class ShoppingCart : Screen
{
    readonly IMessageSession messageSession;

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
        Console.WriteLine("Patterns of Enterprise Application Architecture      $42.00" +
                          Order.Quantity.ToString().PadLeft(14));
        Console.WriteLine("-------------------------------------------------------------------------");
        var totalPrice = "$" + (Order.Price * Order.Quantity).ToString();
        Console.WriteLine("                                                     Total:" + totalPrice.PadLeft(14));
        Console.WriteLine("Press + or - to increase/decrease quantity");
        Console.WriteLine("Press [ENTER] to continue...");
    }


    public override Task<FollowUpAction> HandleKeyPress(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.Enter:
                // We don't do anything here yet, until the summary screen
                return Task.FromResult(FollowUpAction.ScreenIsDone);
            case ConsoleKey.OemPlus:
                if (Order.Quantity < 10) Order.Quantity++;
                Display();
                break;
            case ConsoleKey.OemMinus:
                if (Order.Quantity > 1) Order.Quantity--;
                Display();
                break;
            case ConsoleKey.Delete:
                return Task.FromResult(FollowUpAction.Exit);
        }

        return Task.FromResult(FollowUpAction.WaitForNewKeypress);
    }

    public override int Sequence => 1;
}