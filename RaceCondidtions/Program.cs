var semaphore = new SemaphoreSlim(0);
int number = 0;
List<Task> tasks = [];
var semaphoreWaitTask = semaphore.WaitAsync();

var incrementFunc = async () =>
{
    // To narrow only to increment operation execution,
    // all waiting for semaphore release
    await semaphoreWaitTask;

    number++;
};

for (int i = 0; i < 1000; i++)
{
    // Schedule a task, execute task, execute increment operation
    //var task = Task.Run(incrementFunc); 
    
    // Race condition
    var task = incrementFunc(); 
    
    tasks.Add(task);
}

semaphore.Release(); // Run all increments at once
await Task.WhenAll(tasks);

Console.WriteLine(number);