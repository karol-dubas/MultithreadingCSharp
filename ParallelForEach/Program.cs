using System.Diagnostics;

// The Parallel executes operations by dividing work into chunks and distributing it across multiple threads,
// aiming to use all available processors efficiently.
// Concurrency is more efficient when the tasks involve waiting for external events, such as I/O operations.
// In such cases, threads can be yielded to perform other tasks while waiting, making better use of system resources.
// This approach minimizes idle time and improves scalability.

// In contrast, Task.WhenAll simply waits for all specified asynchronous tasks to complete,
// without controlling how those tasks are distributed or executed.
// Parallelism is more effective for tasks that are computationally intensive and can be executed simultaneously.
// In these scenarios, threads cannot be yielded, as the workload requires continuous processing.
// Therefore, parallelism scales better with tasks that demand heavy computation.

var numbers = Enumerable.Range(1, 10_000_000);
const long validSum = 3203324994356;

// Synchronous calculation (single threaded)
long sum = 0;
var stopwatch = Stopwatch.StartNew();

foreach (int number in numbers)
{
    if (IsPrime(number))
        sum += number;
}

stopwatch.Stop();

Console.WriteLine($"Single threaded: \t" +
                  $"valid = {sum == validSum}, " +
                  $"sum = {sum}, " +
                  $"time = {stopwatch.ElapsedMilliseconds} ms");

// Parallel calculation (multi threaded)
sum = 0;
object @lock = new { };
stopwatch.Restart();

Parallel.ForEach(numbers, number =>
{
    if (!IsPrime(number)) 
        return;
    
    lock (@lock)
    {
        sum += number;
    }
});

stopwatch.Stop();

Console.WriteLine($"Multi threaded (lock): \t" +
                  $"valid = {sum == validSum}, " +
                  $"sum = {sum}, " +
                  $"time = {stopwatch.ElapsedMilliseconds} ms");

// Parallel calculation (multi threaded optimized)
sum = 0;
stopwatch.Restart();

Parallel.ForEach(numbers, number =>
{
    if (IsPrime(number))
        Interlocked.Add(ref sum, number);
});

stopwatch.Stop();

Console.WriteLine($"Multi threaded (opti.): " +
                  $"valid = {sum == validSum}, " +
                  $"sum = {sum}, " +
                  $"time = {stopwatch.ElapsedMilliseconds} ms");

// Asynchronous + parallel calculation (multi threaded)
sum = 0;
SemaphoreSlim sem = new(1, 1); // not needed here and makes it slower
stopwatch.Restart();

await Parallel.ForEachAsync(numbers, async (number, _) =>
{
    if (!IsPrime(number)) 
        return;

    await sem.WaitAsync(); 
    
    try
    {
        sum += number;
    }
    finally
    {
        sem.Release();
    }
});

stopwatch.Stop();

Console.WriteLine($"Multi threaded (async): " +
                  $"valid = {sum == validSum}, " +
                  $"sum = {sum}, " +
                  $"time = {stopwatch.ElapsedMilliseconds} ms");

static bool IsPrime(int number)
{
    if (number <= 1)
        return false;
    
    if (number == 2)
        return true;
    
    if (number % 2 == 0)
        return false;
    
    int boundary = (int)Math.Floor(Math.Sqrt(number));

    for (int i = 3; i <= boundary; i += 2)
    {
        if (number % i == 0)
            return false;
    }
    
    return true;
}
