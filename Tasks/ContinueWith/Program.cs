using Helpers;

// ContinueWith compared to async & await is harder to read, has more code and is more error-prone.
// However, it allows for the continuations of a task that wasn't awaited (fire and forget).

ThreadExtensions.PrintCurrentThread(1);

var loadLinesTask = Task.Run(async () =>
{
    ThreadExtensions.PrintCurrentThread(3);
    await Task.Delay(500);
    return 1;
});

// ContinueWith allows for a continuation, and it will run when task has finished
var processStocksTask = loadLinesTask.ContinueWith(completedTask =>
{
    ThreadExtensions.PrintCurrentThread(4);

    // Task has completed, so using Result is fine here,
    // it won't lock any thread, it just contains what the task returns
    int number = completedTask.Result;
    Console.WriteLine($"Result: {number}");
});

ThreadExtensions.PrintCurrentThread(2);
Console.ReadKey();
