int result = 0;

// Spawning and disposing a Thread with new keyword has a cost, because of OS activities
var thread1 = new Thread(() =>
{
    Thread.Sleep(500); // Simulate long-running operation
    
    // TODO: closures, is this below true?
    // This lambda captures the outer variable 'result'
    // The compiler transforms this into a closure, enabling access to 'result'
    result = 1; // The lambda sets 'result' in its closure
});
thread1.Start();

Console.WriteLine(result);
thread1.Join(); // Blocking call, wait for the Thread to finish
Console.WriteLine(result);


// Handling exceptions
Exception? threadException = null;
var thread2 = new Thread(() =>
{
    try
    {
        Thread.Sleep(500); // Simulate operation
        throw new Exception("Inner thread exception");
    }
    catch (Exception e)
    {
        threadException = e;
    }
});
thread2.Start();
thread2.Join();

if (threadException is not null)
{
    Console.WriteLine();
    Console.WriteLine($"Exception with message '{threadException.Message}' occured during thread execution");
}

// Task benefits:
// - Exception rethrow/handling and generic results (no need to create external variables).
// - Async programming with await (multiple threads used) which solves the thread starvation problem,
//   because Task is a promise of an operation completing in the future.
//   Easy non-blocking WhenAll & WhenAny methods.
// - Easy continuation chaining.
