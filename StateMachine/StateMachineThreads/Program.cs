using Helpers;

ThreadExtensions.PrintCurrentThread(1);
await Test(); // Comment/uncomment await to see the difference
ThreadExtensions.PrintCurrentThread(4); // Continues on the same thread (await changes it)
Console.ReadKey();

async Task Test()
{
    ThreadExtensions.PrintCurrentThread(2); // Still the same thread (awaited or not)
    
    // Task was returned (handle to that method execution) via the async state machine,
    // and this runs in parallel on another thread.
    await Task.Delay(500);
    
    // Continuation on previous thread depends on SynchronizationContext.
    // Unlike desktop apps, console & ASP.NET (new) don't have it.
    Console.WriteLine($"SynchronizationContext: {SynchronizationContext.Current}");
    
    ThreadExtensions.PrintCurrentThread(3); // No SynchronizationContext means continuing on a random ThreadPool's thread.
}
