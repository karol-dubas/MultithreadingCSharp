ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
ThreadPool.GetAvailableThreads(out int avWorkerThreads, out int avCompletionPortThreads);

ThreadPool.QueueUserWorkItem(_ => Console.WriteLine("Hello from ThreadPool"));

Console.WriteLine("End");