using System.Diagnostics;

var repo = new Repository();
var sw = Stopwatch.StartNew();

var fooTask = repo.GetFooAsync();
var barTask = repo.GetBarAsync();

Console.WriteLine(fooTask.IsCompleted); // False

// Blocking operations that waits for an asynchronous operation to finish (fooTask.IsCompleted == true)
// these will run synchronously and may cause a deadlock (if app has a SynchronizationContext):
//int fooResult = fooTask.Result;
//fooTask.Wait();

int[] results = await Task.WhenAll(fooTask, barTask); // Result array is returned

Console.WriteLine(fooTask.IsCompleted); // True
Console.WriteLine(sw.ElapsedMilliseconds); // ~1000ms elapsed

int result = results.Sum(); // Getting data with an array

// Alternative with Task.Result.
// It was awaited, so the Task is completed, and it's safe to use Task.Result
int resultAlt = fooTask.Result + barTask.Result;

Console.WriteLine();

class Repository
{
    public async Task<int> GetFooAsync()
    {
        await Task.Delay(1000);
        return 1;
    }
    
    public async Task<int> GetBarAsync()
    {
        await Task.Delay(500);
        return 2;
    }
}