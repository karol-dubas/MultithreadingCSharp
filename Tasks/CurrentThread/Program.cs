Console.WriteLine($"Number of logical threads: {Environment.ProcessorCount}");

Console.WriteLine("[1] Thread:" + Environment.CurrentManagedThreadId);

Task.Run(() =>
{
    Console.WriteLine("[2] Thread:" + Environment.CurrentManagedThreadId);
    
    Task.Run(() =>
    {
        Console.WriteLine("[2.1] Thread:" + Environment.CurrentManagedThreadId);
    });
});

Console.WriteLine("[3] Thread:" + Environment.CurrentManagedThreadId);