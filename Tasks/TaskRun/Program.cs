using Helpers;

// Task.Run Queues delegates in the TaskScheduler and later in ThreadPool for execution.

// Unlike async & await, which changes thread on first encountered await,
// Task.Run schedules delegate on a new thread immediately (assuming that ThreadPool isn't busy).
ThreadExtensions.PrintCurrentThread(1);

Task.Run(() =>
{
    ThreadExtensions.PrintCurrentThread(2);

    Thread.Sleep(500);
    Console.WriteLine("1");
});

int result = await Task.Run(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("2");
    return 1; // Task<T> with result
});

Task.Run(async () => // Anonymous async method automatically returns a Task, not void, so it's async Task behind
{
    await Task.Delay(1500);
    Console.WriteLine("3");
});

Console.WriteLine("end");
Console.ReadKey();