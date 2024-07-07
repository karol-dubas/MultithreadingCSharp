Console.WriteLine($"CPU logical threads: {Environment.ProcessorCount}");

Console.WriteLine();
Console.WriteLine("Manual Thread:");

// Creating a Thread is an expensive operation.
var thread = new Thread(() =>
{
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} started");
    Thread.Sleep(500);
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} finished");
});

thread.Start();
thread.PrintBasicInfo();
thread.Join(); // Block main thread and wait

Console.WriteLine();
Console.WriteLine("ThreadPool Thread:");

// parallelism

// Threads in a ThreadPool are created on-demand, when the app needs them.
// ThreadPool optimizes operations on threads by reusing, recycling them, distributes work evenly, etc.
// enhancing multithreading performance and leading to efficient use of system resources.
bool tpThreadFinished = false;
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} started");
    Thread.CurrentThread.PrintBasicInfo();
    Thread.Sleep(500);
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} finished");
    tpThreadFinished = true;
});

while (!tpThreadFinished) { }

// Types of threads:
//   - Worker Threads
//   - I/O completion port thread

ThreadPool.GetMinThreads(out int minWorkerThreads, out int minCompletionPortThreads);
ThreadPool.GetMaxThreads(out int maxWorkerThreads, out int maxCompletionPortThreads);
ThreadPool.GetAvailableThreads(out int avWorkerThreads, out int avCompletionPortThreads);

Console.WriteLine();
Console.WriteLine($"Worker threads: \t min: {minWorkerThreads} \t max: {maxWorkerThreads} \t available: {avWorkerThreads}");
Console.WriteLine($"I/O threads: \t\t min: {minCompletionPortThreads} \t max: {maxCompletionPortThreads} \t available: {avCompletionPortThreads}");
