var numberHandlerTask = GetNumberHandler();
Console.WriteLine($"{nameof(numberHandlerTask)} status: '{numberHandlerTask?.Status}'"); // WaitingForActivation
int number = await numberHandlerTask!;
Console.WriteLine($"{nameof(numberHandlerTask)} status: '{numberHandlerTask?.Status}'"); // RanToCompletion
Console.WriteLine(number);

async Task<int> GetNumberHandler()
{
    Task<int> getNumberTask = null!;
    
    try
    {
        getNumberTask = GetNumber();
        
        // If task throws an exception on execution,
        // then it will be re-thrown (await) and caught here.
        return await getNumberTask;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Caught exception of type '{ex.GetType()}' with message: '{ex.Message}'");
    }
    
    // State machine sets task status to Faulted if exception was caught.
    // AggregateException is produced when working with tasks.
    Console.WriteLine($"{nameof(getNumberTask)} status: '{getNumberTask?.Status}', " +
                      $"with exception type '{getNumberTask?.Exception?.GetType()}' and message: '{getNumberTask?.Exception?.Message}'"); 

    return -1;
}

async Task<int> GetNumber()
{
    await Task.Delay(100);
    throw new Exception("Operation failed");
    return 1;
}
