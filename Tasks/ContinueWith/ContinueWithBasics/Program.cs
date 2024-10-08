﻿using Helpers;

// ContinueWith compared to async & await is harder to read, has more code and is more error-prone.
// However, it allows for the continuation of a task that wasn't awaited (fire and forget).

ThreadExtensions.PrintCurrentThread(1);

var getNumberTask = Task.Run(async () =>
{
    ThreadExtensions.PrintCurrentThread(3);
    await Task.Delay(500);
    return 1;
});

// ContinueWith allows for a continuation, and it will run when the task has finished
var printNumberTask = getNumberTask.ContinueWith(completedTask =>
{
    ThreadExtensions.PrintCurrentThread(4);

    // The Task has completed, so using Result is fine here.
    // It won't lock any thread, it just contains what the task returns.
    int number = completedTask.Result;
    Console.WriteLine($"Result: {number}");
});

ThreadExtensions.PrintCurrentThread(2);

await printNumberTask;
