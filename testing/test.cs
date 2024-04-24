using System;
using System.Timers;

class Program
{
    static Timer myTimer = new Timer(1000);

    static void Main()
    {
        myTimer.Elapsed += UpdateTime;
        myTimer.Enabled = true;

        Console.WriteLine("Press any key to stop...");
        Console.ReadLine();
    }

    private static void UpdateTime(Object source, ElapsedEventArgs e)
    {
        Console.WriteLine("Current time: {0:HH:mm:ss}", e.SignalTime);
    }
}
