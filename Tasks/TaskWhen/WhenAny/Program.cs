var timeoutTask = Task.Delay(1_015);

var task1 = Task.Run(async () => await Task.Delay(500));
var task2 = Task.Run(async () => await Task.Delay(1_000));
var waitAllTask = Task.WhenAll(task1, task2);

// Returns Task after the completion of any first task.
// In this demo it is used as a timeout mechanism, but it's easier to use CancellationTokenSource.CancelAfter to achieve a timeout.
var firstCompletedTask = await Task.WhenAny(waitAllTask, timeoutTask);

Console.WriteLine(firstCompletedTask == timeoutTask 
        ? "Timed out" 
        : "Completed successfully");