namespace ThreadVsTask;

public class TaskExample
{
    private object _lock = new { };
    SemaphoreSlim _startSem = new(0);
    private Service _service = new();

    public async Task Run(int taskCount)
    {
        async Task Fn()
        {
            await _startSem.WaitAsync();

            // The lock blocks the Task.
            // Blocking the Task blocks the Thread, and blocked Thread can't pick up any other Task.
            // Next Tasks will wait here and block attached Threads.
            // The CPU will try to perform operations on the Thread that is blocked,
            // and in effect, it won't do anything.
            // If there are more Tasks than threads, it will cause a deadlock.
            lock (_lock)
            {
                _service.DoWork().GetAwaiter().GetResult();
            }
        }

        List<Task> tasks = [];
        
        for (int i = 0; i < taskCount; i++)
        {
            // Send the Task to the ThreadPool, which will assign a thread to execute it.
            // Task -> TaskScheduler -> ThreadPool -> Thread -> CPU.
            // ThreadPool manages to create the appropriate number of threads needed to run created tasks,
            // but we don't know how many Threads will be used.
            // CPU can switch threads, but ThreadPool can't do the same with tasks.
            // Once it's attached, it can't be detached.
            var task = Task.Run(Fn);
            
            tasks.Add(task);
        }

        _startSem.Release(taskCount);
        await Task.WhenAll(tasks);
    }
}