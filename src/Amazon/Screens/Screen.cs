namespace Amazon.Screens;

public enum FollowUpAction
{
    WaitForNewKeypress,
    ScreenIsDone,
    Exit
}

public abstract class Screen
{
    public abstract void Display();
    public abstract Task<FollowUpAction> HandleKeyPress(ConsoleKey key);
    public abstract int Sequence { get; }
    public Order Order { get; set; }
}
