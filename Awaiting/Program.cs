var fileRepository = new FileRepository();
var dbRepository = new DatabaseRepository();

var dbResultTask = dbRepository.GetAsync(); // No await, continue
int fileResult = await fileRepository.GetAsync(); // Again return control to the caller

class FileRepository
{
    public async Task<int> GetAsync()
    {
        Thread.Sleep(500);
        return 1;
    }
}

class DatabaseRepository
{
    public async Task<int> GetAsync()
    {
        await Task.Delay(1000); // Return control to the caller
        return 2;
    }
}