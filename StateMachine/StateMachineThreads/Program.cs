Console.WriteLine($"Start 1: {Environment.CurrentManagedThreadId}");
await Test();
Console.WriteLine($"End 1: {Environment.CurrentManagedThreadId}");
Console.ReadKey(false);

// TODO: no await version (compare thread ids)

// Console.WriteLine($"Start 2: {Environment.CurrentManagedThreadId}");
// Task.Run(Test);
// Console.WriteLine($"End 2: {Environment.CurrentManagedThreadId}");
// Console.ReadKey(false);

async Task Test(int param = 1)
{
    // Copies whole stack: local variables, params, "this" variables and contexts
    int i = 1; 
    
    Console.WriteLine($"Before: {Environment.CurrentManagedThreadId}");
    await Task.Delay(1_000); // Task is returned and this runs in parallel on another thread
    
    // It depends on SynchronizationContext, console & ASP.NET Core apps don't have it, WPF has 
    //var sc = SynchronizationContext.Current;
    
    Console.WriteLine($"After: {Environment.CurrentManagedThreadId}"); // TODO: different thread
    
    i = 2; // Stack is restored after await
}