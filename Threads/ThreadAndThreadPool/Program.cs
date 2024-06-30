Console.WriteLine("Manual Thread:");

// Creating a Thread is an expensive operation.
var thread = new Thread(() =>
{
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} started");
    Thread.Sleep(1000);
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} finished");
});

thread.Start();
thread.PrintBasicInfo();
thread.Join(); // Block main thread and wait

Console.WriteLine();
Console.WriteLine("ThreadPool Thread:");

// ThreadPool optimizes operations on threads by reusing, recycling them etc.
// enhancing multithreading performance.
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} started");
    Thread.CurrentThread.PrintBasicInfo();
    Thread.Sleep(1000);
    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId} finished");
});

Console.ReadKey();