Console.WriteLine("Press [ENTER] to execute");

var calc = new Client();
var rnd = new Random();

#region First event handler
// calc.ThresholdReached += CalcOnThresholdReached;
//
// void CalcOnThresholdReached(object? sender, EventArgs eventArgs)
// {
//     Console.WriteLine("An event was just fired!");
// }
#endregion

#region Second event handler
// calc.ThresholdReached += delegate
// {
//     Console.WriteLine("We've done enough calculations, I'm quitting");
//     Environment.Exit(0);
// };
#endregion

while (true)
{
    if (!Console.KeyAvailable) continue;
    var userInput = Console.ReadKey(true);

    var result = calc.Add(1295, rnd.Next(40,44));
    Console.WriteLine($"The result is: {result}");
}