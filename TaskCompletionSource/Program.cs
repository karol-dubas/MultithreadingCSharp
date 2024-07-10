ThreadExecutor threadExecutor = new();

Console.WriteLine("Start");

const int count = 5;
for (int i = 0; i < count; i++)
{
    int copy = i;
    threadExecutor.Enqueue(delegate
    {
        Console.WriteLine($"[{copy}] Distributed work started...");
        Thread.Sleep(Random.Shared.Next(2_000));
    });
}
await threadExecutor.ExecuteAndWaitAll();
Console.WriteLine("End");