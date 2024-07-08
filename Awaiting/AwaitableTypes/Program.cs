// `await` can be used only on objects with GetAwaiter method that returns INotifyCompletion object
int number = await Get();
Console.WriteLine(number);

Task<int> Get() // No async
{
    var storage = new Storage();
    return storage.GetNumber(); // Just forwarding a Task
}

class Storage
{
    public async Task<int> GetNumber()
    {
        await Task.Delay(1_000); // Simulate delay
        return 1;
    }
}