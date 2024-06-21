var semaphore = new SemaphoreSlim(1);

for (int i = 0; i < 3; i++)
{
    Console.WriteLine($"[{i}] Start");
    var waitTask = semaphore.WaitAsync();

    if (!waitTask.IsCompleted)
        Console.WriteLine($"[{i}] Waiting for a semaphore release...");

    await waitTask;
    
    Console.WriteLine($"[{i}] Allowed");
    
    Task.Run(async () =>
    {
        await Task.Delay(1000);
        semaphore.Release();
    });
}
