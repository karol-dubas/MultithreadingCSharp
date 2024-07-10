using System.Collections.Concurrent;

/// <summary>
/// Class adapted to a new Task-based asynchronous programming approach.
/// </summary>
public class AsyncThreadExecutor : ThreadExecutor
{
    // Used for old, legacy code (different from TAP approach) to create awaitable Task.
    // It doesn't run anything itself, it is marked as completed/failed when it gets the result set on it.
    // Property Task can be returned or awaited.
    private readonly TaskCompletionSource _tcs = new();
    
    public Task ExecuteAllAsync()
    {
        new Thread(() =>
        {
            ExecuteAll();
            _tcs.SetResult();
        }).Start();

        return _tcs.Task;
    }
}

/// <summary>
/// Not editable, legacy code.
/// </summary>
public class ThreadExecutor
{
    private readonly ConcurrentBag<Thread> _threads = [];

    public void Prepare(Action action)
    {
        var thread = new Thread(() => action());
        _threads.Add(thread);
    }

    public void ExecuteAll()
    {
        foreach (var thread in _threads)
            thread.Start();
        
        foreach (var thread in _threads)
            thread.Join();
        
        _threads.Clear();
    }
}

