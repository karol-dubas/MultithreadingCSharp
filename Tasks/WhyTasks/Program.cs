// Task is a promise of an operation completing in the future

// TODO: https://code-maze.com/csharp-tasks-vs-threads/

int result = 0;

// Spawning and disposing a Thread with new keyword has a cost, because of OS activities
var thread = new Thread(() =>
{
    Thread.Sleep(2000); // Simulate long-running operation
    result = 1;
});
thread.Start();

Console.WriteLine(result);
thread.Join(); // Blocking call, wait for the Thread to finish
Console.WriteLine(result);