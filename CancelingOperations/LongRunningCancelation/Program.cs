var cts = new CancellationTokenSource();
cts.Token.Register(() => Console.WriteLine("Cancellation was triggered somewhere")); // Register cancel event

Console.WriteLine("Starting counter...");
var counterTask = RunCounter(cts.Token);

Console.WriteLine("Press any key to cancel operation...");
Console.ReadKey(true);
cts.Cancel();
await counterTask;
return;

Task RunCounter(CancellationToken ct)
{
    return Task.Run(async () =>
    {
        Console.WriteLine("Counter started");
        int i = 0;
        while (true)
        {
            if (ct.IsCancellationRequested)
                break;
            Console.WriteLine(++i);
            await Task.Delay(1000);
        }
        Console.WriteLine("Counter canceled");
    }, ct);
}