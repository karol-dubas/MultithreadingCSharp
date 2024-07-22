using System.Diagnostics;

var sw = Stopwatch.StartNew();

var fooTask = GetFooAsync();
var barTask = GetBarAsync();

Console.WriteLine(fooTask.IsCompleted); // False

// Instead of awaiting tasks one by one, the data can be loaded in parallel by performing multiple asynchronous operations at the same time.
// Task.WhenAll accepts a tasks collection, it creates and returns a Task,
// which task status is completed only when all the tasks passed to the method are marked as completed.
int[] arrayResult = await Task.WhenAll(fooTask, barTask);

Console.WriteLine(fooTask.IsCompleted); // True
Console.WriteLine(sw.ElapsedMilliseconds); // ~1000ms elapsed

Console.WriteLine(arrayResult.Sum()); // Getting the data from an array

// Getting results one by one from completed tasks with Task.Result.
// Tasks were awaited, it means that they are completed, and it's safe to use Task.Result.
Console.WriteLine(fooTask.Result + barTask.Result);

async Task<int> GetFooAsync()
{
    await Task.Delay(1000);
    return 1;
}

async Task<int> GetBarAsync()
{
    await Task.Delay(500);
    return 2;
}
