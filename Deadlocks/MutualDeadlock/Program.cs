object lock1 = new { };
object lock2 = new { };

var task1 = Task.Run(() =>
{
    lock (lock1)
    {
        Console.WriteLine("Task 1 entered lock 1");
        
        lock (lock2)
        {
            Console.WriteLine("Task 1 entered lock 2");
        }
    } 
});

var task2 = Task.Run(() =>
{
    lock (lock2)
    {
        Console.WriteLine("Task 2 entered lock 2");

        lock (lock1)
        {
            Console.WriteLine("Task 2 entered lock 1");
        }
    } 
});

// Each of the tasks is waiting for the other to finish, causing them to block each other.
// It could be fixed with, for example, SemaphoreSlim with a timeout.

await Task.WhenAll(task1, task2);