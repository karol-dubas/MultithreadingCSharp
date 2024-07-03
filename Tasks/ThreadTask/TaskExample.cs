namespace ThreadTask;

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

            // Task attached to a Thread blocks the execution of the Thread.
            // Blocking the Task blocks the Thread, blocked Thread can't pick up any other Task.
            // Next tasks will wait here and block attached Thread.
            // If there are more Tasks than Threads it will cause a deadlock.
            // The CPU will try to perform operations on the Thread that is blocked and in effect it won't do anything.
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
            // ThreadPool manages creating appropriate amount of threads needed to run created tasks,
            // but we don't know how many Threads will be used.
            // CPU can switch threads, but ThreadPool can't do the same with tasks, once it's attached it can't be detached.
            var task = Task.Run(Fn);
            
            tasks.Add(task);
        }

        _startSem.Release(taskCount);
        await Task.WhenAll(tasks);
    }
}