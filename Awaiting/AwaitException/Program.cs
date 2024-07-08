int number = await Get();
Console.WriteLine(number);

async Task<int> Get()
{
    Task<int> getNumberTask = null!;
    
    try
    {
        var storage = new Storage();
        getNumberTask = storage.GetNumber();
        
        // If task throws an exception on execution,
        // then it will be re-thrown (await) and caught here.
        return await getNumberTask;
    }
    catch (Exception ex)
    {
        // State machine sets task status to Faulted
        Console.WriteLine($"Task status: {getNumberTask?.Status}, exception: {getNumberTask?.Exception?.Message}");
    }

    return -1;
}

class Storage
{
    public async Task<int> GetNumber()
    {
        await Task.Delay(100);
        throw new Exception("Operation failed");
        return 1;
    }
}