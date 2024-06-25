public class FileNumberService
{
    private readonly SemaphoreSlim _fileSaveSemaphore = new(1);
    private const string Path = "number.txt";
    
    public FileNumberService()
    {
        File.WriteAllText(Path, "0");
    }

    public async Task IncrementNumber()
    {
        // Only 1 task can access the file
        await _fileSaveSemaphore.WaitAsync();

        string text = await File.ReadAllTextAsync(Path);
        int number = int.Parse(text);
        number++;
        await File.WriteAllTextAsync(Path, number.ToString());
        
        _fileSaveSemaphore.Release();
    }

    public async Task<int> ReadNumber()
    {
        string text = await File.ReadAllTextAsync(Path);
        return int.Parse(text);
    }
}