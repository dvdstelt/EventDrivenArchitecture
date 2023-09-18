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
}
