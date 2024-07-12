var ct = new CancellationToken(canceled: true);

var task = Task.Run(() => Console.WriteLine("Not invoked"), ct);

task.ContinueWith(t => Console.WriteLine("Not invoked"), ct);

task.ContinueWith(async t =>
    {
        await Task.Delay(500);
        Console.WriteLine($"I canceled this task. Status: {t.Status}");
    }, 
    TaskContinuationOptions.OnlyOnCanceled)
    .ContinueWith(t => Console.WriteLine($"Canceled continuation task status: {t.Status}")); // Chained one is not canceled.

try
{
    await task; // It's just waiting for this task, not for chained continuations.
}
catch (TaskCanceledException e)
{
    Console.WriteLine($"Cancel exception handled. Message: {e.Message}");
}

Console.ReadKey(); // Wait for chained continuations.