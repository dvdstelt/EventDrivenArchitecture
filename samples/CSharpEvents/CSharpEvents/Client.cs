public class Client
{
    int calculationsDone = 0;
    public event EventHandler ThresholdReached;

    public int Add(int x, int y)
    {
        if (++calculationsDone == 3)
            ThresholdReached?.Invoke(this, EventArgs.Empty);

        return x + y;
    }
}