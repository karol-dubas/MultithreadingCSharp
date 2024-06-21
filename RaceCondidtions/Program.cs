var semaphore = new SemaphoreSlim(0);
int number = 0;
List<Task> tasks = [];
var semaphoreWaitTask = semaphore.WaitAsync();

var incrementFunc = async () =>
{
    // To narrow only to increment operation execution,
    // all waiting for semaphore release
    await semaphoreWaitTask;
    
    Thread.Sleep(10); // To see the "counters" statistics
    
    number++; // Race condition
};

for (int i = 0; i < 10_000; i++)
{
    // Schedule a task, execute task, execute increment operation
    // Tasks are scheduled in parallel
    //var task = Task.Run(incrementFunc); 
    
    // Different from Task.Run,
    // instead of scheduling a task, the task is created
    var task = incrementFunc();
    
    tasks.Add(task);
}

semaphore.Release(); // Run all increment tasks at once
await Task.WhenAll(tasks);

Console.WriteLine(number);