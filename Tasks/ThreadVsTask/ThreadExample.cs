namespace ThreadVsTask;

public class ThreadExample
{
    private object _lock = new { };
    SemaphoreSlim _startSem = new(0);
    private Service _service = new();

    public void Run(int threadCount)
    {
        void Fn()
        {
            _startSem.Wait();

            // It is synchronous like the Task example, but the CPU can switch executing threads,
            // because there is no Task blocking the Thread, therefore, we won't experience a deadlock.
            lock (_lock)
            {
                _service.DoWork().GetAwaiter().GetResult();
            }
        }

        List<Thread> threads = [];
        
        for (int i = 0; i < threadCount; i++)
        {
            // Threads can be described as an opportunity to execute a code when placed on the CPU, which actually runs the code.
            var thread = new Thread(Fn);
            
            threads.Add(thread);
        }

        _startSem.Release(threadCount);

        foreach (var thread in threads)
            thread.Join();
    }
}