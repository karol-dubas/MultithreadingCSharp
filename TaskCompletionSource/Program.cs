AsyncThreadExecutor executor = new();

Console.WriteLine("Start");

const int count = 4;
for (int i = 1; i <= count; i++)
{
    int copy = i;
    executor.Prepare(delegate
    {
        Console.WriteLine($"[{copy}] Started working...");
        Thread.Sleep(Random.Shared.Next(2_000));
        Console.WriteLine($"[{copy}] Completed");
    });
    
    Console.WriteLine($"Prepared {i}/{count}");
}

// TODO: What is the benefit of this?
// Main thread isn't blocked, but a new waiting thread is created instead
await executor.ExecuteAllAsync(); 

Console.WriteLine("End");