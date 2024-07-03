namespace ThreadTask;

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
    }
}