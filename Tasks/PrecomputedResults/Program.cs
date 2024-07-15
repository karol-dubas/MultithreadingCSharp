// All methods complete immediately, async ones would be awaited
await GetTask();

int result = await GetResultTask();
Console.WriteLine($"Result: {result}");

try
{
    await GetExceptionTask();
}
catch (Exception e)
{
    Console.WriteLine($"Exception caught. Message: {e.Message}");
}

CancellationTokenSource cts = new();
cts.Cancel();
var canceledTask = GetCanceledTask(cts.Token); // await will re-throw canceled exception
Console.WriteLine($"Status: {canceledTask.Status}");
return;

// All methods below aren't async, no need to spawn a unnecessary state machine.
// Everything is completed, with no method signature change.
// These Task methods create a Task which is instantly marked as completed/faulted/canceled with the specified result,
// so these are just a Task result wrappers.

Task GetTask()
{
    return Task.CompletedTask;
}

Task<int> GetResultTask()
{
    return Task.FromResult(1);
}

// No need to schedule a Task that returns known value
Task<int> InvalidGetResultTask()
{
    return Task.Run(() => 1);
}

Task<int> GetExceptionTask()
{
    var exception = new Exception("oops");
    return Task.FromException<int>(exception);
    return Task.FromResult(1);
}

Task GetCanceledTask(CancellationToken ct)
{
    return ct.IsCancellationRequested 
        ? Task.FromCanceled(ct) 
        : Task.CompletedTask;
}

