var startSemaphore = new SemaphoreSlim(0);
const int totalRuns = 1_000;
var numberService = new FileNumberService();
List<Task> tasks = [];
var semaphoreWaitStartTask = startSemaphore.WaitAsync();

var incrementFunc = async () =>
{
    await semaphoreWaitStartTask;
    await numberService.IncrementNumber();
};

for (int i = 0; i < totalRuns; i++)
{
    var task = incrementFunc();
    tasks.Add(task);
}

startSemaphore.Release();
await Task.WhenAll(tasks);

Console.WriteLine($"{await numberService.ReadNumber()}/{totalRuns}");