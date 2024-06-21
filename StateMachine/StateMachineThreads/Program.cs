// Console.WriteLine($"Start 1: {Environment.CurrentManagedThreadId}");
// await Test();
// Console.WriteLine($"End 1: {Environment.CurrentManagedThreadId}");
// Console.ReadKey(false);

Console.WriteLine($"Start 2: {Environment.CurrentManagedThreadId}");
Task.Run(Test); // TODO: vs `Test`, different threads on "Before"
Console.WriteLine($"End 2: {Environment.CurrentManagedThreadId}");
Console.ReadKey(false);

async Task Test()
{
    int i = 1;
    Console.WriteLine($"Before: {Environment.CurrentManagedThreadId}");
     await Task.Delay(100);
    Console.WriteLine($"After: {Environment.CurrentManagedThreadId}");
    i = 2;
}