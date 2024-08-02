for (int i = 0; i < 10; i++)
{
    var cts = new CancellationTokenSource();
    cts.CancelAfter(100);
    
    try
    {
        // WaitAsync can be used to cancel the Task when a method doesn't have CancellationToken parameter.
        // Executing Task will complete anyway, but code execution will continue.
        // It is used as a timeout.
        int result = await GetNumberAsync().WaitAsync(cts.Token);
        Console.WriteLine($"Result: {result}");
    }
    catch (OperationCanceledException e)
    {
        Console.WriteLine(e.Message);
    }
}

Console.WriteLine("Finished");

// Non-editable method
async Task<int> GetNumberAsync()
{
    await Task.Delay(105);
    return Random.Shared.Next(100);
}