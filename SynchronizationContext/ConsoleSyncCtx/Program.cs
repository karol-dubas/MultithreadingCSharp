var sc = SynchronizationContext.Current;
Console.WriteLine($"Is SynchronizationContext null: {sc is null}"); // doesn't exists

await RunOne();
//await RunTwo();
Console.WriteLine();
return;

async Task RunOne()
{
    PrintCurrentThread(1);

    await Task.Run(async () =>
    {
        PrintCurrentThread(2);

        await Task.Run(() =>
        {
            PrintCurrentThread(3);
        });
    
        PrintCurrentThread(4); // continues execution on the latest used thread
    });

    PrintCurrentThread(5); // continues execution on the latest used thread
}

async Task RunTwo()
{
    PrintCurrentThread(1);

    await Task.Run(async () =>
    {
        PrintCurrentThread(2);

        await Task.Run(() =>
        {
            PrintCurrentThread(3);
        }).ConfigureAwait(false); // no effect
    
        PrintCurrentThread(4);
    }).ConfigureAwait(false); // no effect

    PrintCurrentThread(5);
}

void PrintCurrentThread(int i) => Console.WriteLine($"[{i}] Thread: " + Environment.CurrentManagedThreadId);
