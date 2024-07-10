internal class ThreadWrapper(Thread thread, bool completed)
{
    public bool Completed { get; set; } = completed;
    public Thread Thread { get; } = thread;
}

public class ThreadExecutor
{
    // Used for old, legacy code (different from TAP approach) to create awaitable Task.
    // It doesn't run anything itself, it is marked as completed/failed when it gets the result set on it.
    // Property Task can be returned or awaited.
    private readonly TaskCompletionSource _tcs = new();
    
    private readonly Dictionary<Guid, ThreadWrapper> _threadRegistry = [];

    public void Enqueue(Action action)
    {
        var uniqueId = Guid.NewGuid();

        var thread = new Thread(() =>
        {
            action();
            _threadRegistry[uniqueId].Completed = true;
            Console.WriteLine($"Thread '{uniqueId}' completed");
        });

        _threadRegistry.Add(uniqueId, new ThreadWrapper(thread, false));
    }

    public Task ExecuteAndWaitAll()
    {
        foreach (var threadWrapper in _threadRegistry.Values)
            threadWrapper.Thread.Start();

        new Thread(() =>
        {
            while (true)
            {
                if (!_threadRegistry.Values.All(tw => tw.Completed))
                {
                    Thread.Sleep(10);
                    continue;
                }

                _tcs.SetResult();
                break;
            }
        }).Start();

        return _tcs.Task;
    }
}